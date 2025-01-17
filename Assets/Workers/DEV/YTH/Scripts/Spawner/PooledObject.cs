using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using static MonsterData;

/// <summary>
/// 보스는 Instantiate로 생성해서 따로 관리 고려중..
/// </summary>
public class PooledObject : MonoBehaviour, IKnockBack, IDamagable
{
    [HideInInspector]
    [Inject] public PlayerController player;

    public event Action OnDie;

    public event Action<float, float> OnDamage;

    private MonsterData _monsterData;
    public MonsterData MonsterData {  get { return _monsterData; } }

    private MonsterView _monsterView;
    public MonsterView MonsterView { get { return _monsterView; } }

    private Rigidbody _rigid;

    private Animator _animator;

    private AutoLockOn _autoLockOn;

    private DissolveController _dissolve;

    private MonsterSkillManager _skill;

    private CapsuleCollider _capsuleCollider;
    public CapsuleCollider CapsuleCollider { get { return _capsuleCollider; } set { _capsuleCollider = value; } }

    [Header("Drop Item")]
    [SerializeField] GameObject _gear;

    [SerializeField] GameObject _chip;

    [SerializeField] GameObject _blueChip;

    [SerializeField] GameObject _redChip;

    [Inject] Transform dropPool;

    [Inject] DamagePopUpManager _damagePopUpManager;

    [Inject] InGameSaveData saveData;

    private void Awake()
    {
        _autoLockOn = player.GetComponent<AutoLockOn>();
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _monsterData = GetComponent<MonsterData>();
        _skill = GetComponent<MonsterSkillManager>();
        _dissolve = GetComponent<DissolveController>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        _monsterView = FindAnyObjectByType<MonsterView>(FindObjectsInactive.Include);
    }

    private void OnEnable()
    {
        _dissolve.DissolveReset();
    }


    public void TakeDamage(float damage)
    {
        EffectManager.instance.ParticlePlay("FX_splash_hit_01_air", 1f, this.transform.position, this.transform.rotation);

        if (_monsterData.IsDead)
            return;

        RotateToPlayer();

        // 크리티컬
        damage *= Util.IsRandom(player.Stat.GetAbility(AdditionAbility.Critical)) ? (2 + (player.Stat.GetAbility(AdditionAbility.CriticalDamage) * 0.01f)) : 1;

        _damagePopUpManager?.ShowDamagePopUp(transform.position + Vector3.up * 2, $"{(int)damage}", Color.white);

        Debug.Log($"몬스터 피격 : {damage}");
        _monsterData.CurHp -= damage;
        OnDamage?.Invoke(_monsterData.CurHp, _monsterData.MaxHp);

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
            Die();
            return;
        }

        if (_monsterData.MonsterTIer == MonsterData.MonsterTier.Boss)
            return;

        SoundManager.PlaySFX(SoundManager.Instance.monsterSoundDic[_monsterData.TakeDamageID]);
        _animator.SetTrigger("TakeDamage");
    }

    public void Die()
    {
        OnDie?.Invoke();    

        _monsterData.IsDead = true;
        _animator.SetBool("IsDead", true);

        _capsuleCollider.enabled = false;

        int random = UnityEngine.Random.Range(1, 101);
        Vector3 curPos = new Vector3(transform.position.x, 1f, transform.position.z);

        _autoLockOn.action?.Invoke();
        _animator.SetBool("Move", false);
        _animator.SetTrigger("Die");

       /* StartCoroutine(PreventBug());*/

        SoundManager.PlaySFX(SoundManager.Instance.monsterSoundDic[_monsterData.DieID]);

        Debug.Log(random);

        DropGearItem(random, curPos);
        random = UnityEngine.Random.Range(0, 12);
        DropChipItem(random, curPos + Vector3.right * 0.5f);

        if (_monsterData.MonsterTIer != MonsterData.MonsterTier.Normal)
            DropBlueChip(curPos);

        if (_monsterData.MonsterTIer == MonsterData.MonsterTier.Boss)
            DropRedChip(curPos);

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

   
    IEnumerator PreventBug()
    {
        yield return Util.GetDelay(2f);
        _animator.SetTrigger("Die");
        
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
        transform.LookAt(player.transform.position);
        Debug.Log("돌아봅니다!~~!~!~!~!~!~!~!~!~!~!~!~!~!~!~!");
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
        float dropGearPvalue = 25 * (saveData.chapterSaveData.StageNum + 1);
        switch (_monsterData.MonsterTIer)
        {
            case MonsterData.MonsterTier.Normal:
                if (random > 75)
                {
                    dropGearTier = 1;
                    dropGear = true;
                }
                break;
            case MonsterData.MonsterTier.Elite:
                if (random > 25)
                {
                    dropGearTier = random > 90 ? 3 : random > 75 ? 2 : 1;
                    dropGear = true;
                }
                break;
            case MonsterData.MonsterTier.Boss:
                dropGearTier = random > 65 ? 3 : 2;
                dropGear = true;
                break;
        }
        if (dropGear)
        {
            foreach (var item in dropPool.GetComponentsInChildren<DropGear>(true))
            {
                if (!item.gameObject.activeSelf)
                {
                    item.SetDropItem(dropGearTier, dropGearPvalue);
                    item.transform.position = curPos;
                    item.gameObject.SetActive(true);
                    return;
                }
            }
            Instantiate(_gear, curPos, transform.rotation, dropPool).GetComponent<DropGear>().SetDropItem(dropGearTier, dropGearPvalue);
        }
    }

    private void DropChipItem(float random, Vector3 curPos)
    {
        foreach (var item in dropPool.GetComponentsInChildren<DropChip>(true))
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
            _monsterData.rigid.isKinematic = true;
            _monsterData.rigid.useGravity = false;

            // 범위 타격 실행
            _skill.Explosion(4f, 360f, 50f);
            SoundManager.PlaySFX(SoundManager.SoundData_P.CounterThrowHit);

            // 반격 상황 종료
            _monsterData.IsCountered = false;
        }
    }

    private void DropBlueChip(Vector3 curPos)
    {
        int tier = UnityEngine.Random.Range(1, 4);


        foreach (var item in dropPool.GetComponentsInChildren<DropBlueChip>(true))
        {
            if (!item.gameObject.activeSelf)
            {
                item.DropChipInit(tier);
                item.transform.position = curPos;
                item.gameObject.SetActive(true);
                return;
            }
        }

        if (_monsterData.MonsterTIer == MonsterData.MonsterTier.Elite)
        {
            tier = (UnityEngine.Random.Range(0, 1f)) switch
            {
                < 0.5f => 3,
                <= 0.5f and < 0.8f => 2,
                _ => 1,
            };
        }
        Instantiate(_blueChip, curPos, transform.rotation, dropPool).GetComponent<DropBlueChip>().DropChipInit(tier);
    }

    private void DropRedChip(Vector3 curPos)
    {
        Instantiate(_redChip, curPos, transform.rotation, dropPool).GetComponent<DropRedChip>().DropChipInit();
    }
}
