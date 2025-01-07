using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

/// <summary>
/// 보스는 Instantiate로 생성해서 따로 관리 고려중..
/// </summary>
public class PooledObject : MonoBehaviour, IKnockBack, IDamagable
{
    [HideInInspector]
    [Inject]
    public PlayerController player;

    public event Action OnDie;

    private MonsterData _monsterData;

    private Rigidbody _rigid;

    private Animator _animator;

    private AutoLockOn _autoLockOn;

    private DissolveController _dissolve;

    [Header("Drop Item")]
    [SerializeField] GameObject _gear;

    [SerializeField] GameObject _chip;

    [Inject] Transform dropPool;

    private MonsterSkillManager _skill;

    private void Awake()
    {
        _autoLockOn = player.GetComponent<AutoLockOn>();
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _monsterData = GetComponent<MonsterData>();
        _skill = GetComponent<MonsterSkillManager>();
        _dissolve = GetComponent<DissolveController>();
    }

    private void OnEnable()
    {
        OnDie += Die;
        _dissolve.DissolveReset();
    }

    private void OnDisable()
    {
        OnDie -= Die;
    }

    public void TakeDamage(float damage)
    {
        if (_monsterData.IsDead)
            return;

        RotateToPlayer();

        _rigid.angularVelocity = Vector3.zero;
        _rigid.velocity = Vector3.zero;

        Debug.Log($"몬스터 피격 : {damage}");
        _monsterData.CurHp -= damage;

        if (isAttackedRoutine == null)
        {
            isAttackedRoutine = StartCoroutine(IsAttackedRoutine());
        }
        else
        {
            StopCoroutine(isAttackedRoutine);
            isAttackedRoutine = null;
            isAttackedRoutine = StartCoroutine(IsAttackedRoutine());
        }

        if (_monsterData.CurHp <= 0)
        {
            OnDie?.Invoke();
            return;
        }

        if (_monsterData.MonsterTIer == MonsterData.MonsterTier.Boss)
            return;

        _animator.SetTrigger("TakeDamage");
    }

    public void Die()
    {
        _monsterData.IsDead = true;

        int random = UnityEngine.Random.Range(1, 101);
        Vector3 curPos = new Vector3(transform.position.x, 1f, transform.position.z);

        _autoLockOn.action?.Invoke();
        _animator.SetTrigger("Die");

        Debug.Log(random);

        DropGearItem(random, curPos);
        random = UnityEngine.Random.Range(0, 12);
        DropChipItem(random, curPos + Vector3.right * 0.5f);
    }

    public void DieDissolve()
    {
        StartCoroutine(DissolveRoutine());
    }

    IEnumerator DissolveRoutine()
    {
        _dissolve.StartDissolve();
        yield return Util.GetDelay(_dissolve.ReturnDissolveTime);

        gameObject.SetActive(false);
    }

    public void KnockBack(GameObject attacker)
    {
        if (_monsterData.MonsterTIer == MonsterData.MonsterTier.Boss)
            return;

        StartCoroutine(KnockBackRoutine(attacker));
    }

    IEnumerator KnockBackRoutine(GameObject attacker)
    {
        float knockBackTime = 0.1f;
        Vector3 moveDir = (transform.position - attacker.transform.position).normalized;
        moveDir.y = 0;
        while (true)
        {
            knockBackTime -= Time.deltaTime;
            if (knockBackTime <= 0)
                yield break;

            transform.position += moveDir * 5f * Time.deltaTime;
            yield return null;
        }
    }

    public void RotateToPlayer()
    {
        //피격 시 플레이어 방향으로 회전
        Quaternion lookRot = Quaternion.LookRotation(player.transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, 0.7f * Time.deltaTime); // 속도 빠르게 수정할 것
    }

    Coroutine isAttackedRoutine;
    IEnumerator IsAttackedRoutine()
    {
        _monsterData.IsAttacked = true;
        yield return Util.GetDelay(1f);
        _monsterData.IsAttacked = false;

        isAttackedRoutine = null;
    }

    private void DropGearItem(float random, Vector3 curPos)
    {
        bool dropGear = false;
        int dropGearTier = 1;
        float dropGearPvalue = 0;
        bool dropGearRandomTier = false;
        switch (_monsterData.MonsterTIer)
        {
            case MonsterData.MonsterTier.Normal:
                if (random > 50)
                {
                    dropGearTier = 1;
                    dropGearPvalue = 25f;
                    dropGear = true;
                }
                break;
            case MonsterData.MonsterTier.Elite:
                if (random > 25)
                {
                    dropGearTier = random > 90 ? 3 : random > 75 ? 2 : 1;
                    dropGearPvalue = 50f;
                    dropGear = true;
                }
                break;
            case MonsterData.MonsterTier.Boss:
                dropGearPvalue = 75f;
                dropGear = true;
                break;
        }
        if (dropGear)
        {
            foreach(var item in dropPool.GetComponentsInChildren<DropGear>(true))
            {
                if(!item.gameObject.activeSelf)
                {
                    item.SetDropItem(dropGearTier, dropGearPvalue, dropGearRandomTier);
                    item.transform.position = curPos;
                    item.gameObject.SetActive(true);
                    return;
                }
            }
            Instantiate(_gear, curPos, transform.rotation, dropPool).GetComponent<DropGear>().SetDropItem(dropGearTier, dropGearPvalue, dropGearRandomTier);
        }
    }

    private void DropChipItem(float random, Vector3 curPos)
    {
        foreach(var item in dropPool.GetComponentsInChildren<DropChip>(true))
        {
            if (!item.gameObject.activeSelf)
            {
                item.SetDropChip(random, _monsterData.MonsterTIer != MonsterData.MonsterTier.Boss);
                item.transform.position = curPos;
                item.gameObject.SetActive(true);
                return;
            }
        }
        Instantiate(_chip, curPos, transform.rotation, dropPool).GetComponent<DropChip>().SetDropChip(random, _monsterData.MonsterTIer != MonsterData.MonsterTier.Boss);
    }

    private void OnCollisionEnter(Collision other)
    {
        // 일반 몹이 던져진 후 충돌했을 때
        if (_monsterData.IsCountered 
            && _monsterData.MonsterTIer == MonsterData.MonsterTier.Normal)
        {
            // constraints 복원
            _monsterData.rigid.constraints = RigidbodyConstraints.FreezeAll;
            // 충돌 켜기
            Physics.IgnoreCollision(player.coll, _monsterData.coll, false);

            _monsterData.IsCatched = false;
            _monsterData.agent.enabled = true;

            // 범위 타격 실행
            _skill.Explosion(4f, 360f, 50f);

            // 반격 상황 종료
            _monsterData.IsCountered = false;
        }  
    }
}
