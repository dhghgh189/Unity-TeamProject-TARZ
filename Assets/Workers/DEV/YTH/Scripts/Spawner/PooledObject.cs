using System;
using UnityEngine;

/// <summary>
/// 보스는 Instantiate로 생성해서 따로 관리 고려중..
/// </summary>
public class PooledObject : MonoBehaviour
{
    private ObjectPool _returnPool; //반납 위치
    public ObjectPool ReturnPool { get { return _returnPool; } set { _returnPool = value; } }

    private MonsterData _monsterData;

    public event Action OnDie;

    private AutoLockOn _autoLockOn;

    private void Start()
    {
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
        _monsterData.CurHp -= damage;
        _monsterData.Attacked_First = true;
        // 맞는 애니메이션 재생
        // 넉백 적용

        if (_monsterData.CurHp <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void Die()
    {
        _autoLockOn.action?.Invoke();
        ReturnPool.ReturnPool(this);
        //죽는애니메이션 재생
        // 프리팹 받으면 Instantiate 보상 떨굼
    }
   

   
    
}
