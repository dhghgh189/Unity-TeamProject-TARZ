using System;
using System.Collections;
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

    [Header("Drop Item")]
    [SerializeField] GameObject _gear;

    [SerializeField] GameObject _chip;


    private void Start()
    {
        _autoLockOn = player.GetComponent<AutoLockOn>();

        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _monsterData = GetComponent<MonsterData>();
    }

    private void OnEnable()
    {
        OnDie += Die;
    }

    private void OnDisable()
    {
        OnDie -= Die;
    }

    public void TakeDamage(float damage)
    {
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
        int random = UnityEngine.Random.Range(1, 101);

        _autoLockOn.action?.Invoke();
        _animator.SetTrigger("Die");

        Debug.Log(random);
        switch (_monsterData.MonsterTIer)
        {
            case MonsterData.MonsterTier.Normal:
                if (random > 50)
                    Instantiate(_gear, transform.position + Vector3.up * 0.5f, transform.rotation).GetComponent<DropGear>().SetDropItem(1, 25f);
                break;
            case MonsterData.MonsterTier.Elite:
                if (random > 25)
                    Instantiate(_gear, transform.position + Vector3.up * 0.5f, transform.rotation).GetComponent<DropGear>().SetDropItem(random > 90 ? 3 : random > 75 ? 2 : 1, 50f);
                break;
            case MonsterData.MonsterTier.Boss:
                Instantiate(_gear, transform.position + Vector3.up * 0.5f, transform.rotation).GetComponent<DropGear>().SetDropItem(1, 75f, true);
                break;
        }

        random = UnityEngine.Random.Range(0, 12);
        if (_monsterData.MonsterTIer == MonsterData.MonsterTier.Boss)
        {
            GameObject chip = Instantiate(_chip, transform.position + Vector3.up * 0.5f + Vector3.forward * 0.5f, transform.rotation);
            chip.GetComponent<DropChip>().SetDropChip(random, false);
        }
        else
        {
            GameObject chip = Instantiate(_chip, transform.position + Vector3.up * 0.5f + Vector3.forward * 0.5f, transform.rotation);
            chip.GetComponent<DropChip>().SetDropChip(random, true);
        }

        /* BossMonsterSpwner 일반칩 다른친구드,ㄹ은 그냥 블랙칩*/

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
}
