using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// 보스는 Instantiate로 생성해서 따로 관리 고려중..
/// </summary>
public class PooledObject : MonoBehaviour, IKnockBack, IDamagable
{
    private ObjectPool _returnPool; //반납 위치
    public ObjectPool ReturnPool { get { return _returnPool; } set { _returnPool = value; } }

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

        if (_monsterData.MonsterTIer== MonsterData.MonsterTier.Boss)
            return;

        _animator.SetTrigger("TakeDamage");
    }

    public void Die()
    {
        _autoLockOn.action?.Invoke();
        _animator.SetTrigger("Die");

        GameObject gear = Instantiate(_gear, transform.position + Vector3.up*0.5f, transform.rotation);
        gear.GetComponent<DropGear>().SetDropItem(Part.신발, 1, true, true);

        GameObject chip = Instantiate(_chip, transform.position + Vector3.up * 0.5f + Vector3.forward * 0.5f, transform.rotation);
        chip.GetComponent<DropChip>().SetDropChip(10 * (1 + (player.Stat.GetAbility(AdditionAbility.ChipGetAmount) * 0.01f)), true);

        if (ReturnPool != null)
        {
            ReturnPool.ReturnPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
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
