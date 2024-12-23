using System;
using UnityEngine;
using Zenject;

public class MonsterController : MonoBehaviour/*, IDamagable*/
{
    /*[SerializeField] MonsterData _monsterData;

    [SerializeField] ObjectPool _returnPool;

    [SerializeField] PooledObject _pooledObject;

    [Inject] Inventory _inventory;

    public event Action OnDie;

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
        _monsterData.IsAttacked = true;
        // 맞는 애니메이션 재생
        // 넉백 적용

        if (_monsterData.CurHp <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void Die()
    {
        //죽는애니메이션 재생
        // 원래는 떨어트려야댐 // 프리팹 받으면 Instantiate
    }*/
}
