using System;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private MonsterData _monsterData;

    private ObjectPool _returnPool; //반납 위치
    public ObjectPool ReturnPool { get { return _returnPool; } set { _returnPool = value; } }

    private void Start()
    {
        _monsterData = GetComponent<MonsterData>();
    }

    private void Update() // 사망 처리를 이벤트로 하는게 좋을까..TakeDamage 쪽에서 처리 하지 않으면 결국 똑같은 거 같은데..
    {
        if (_monsterData.CurHp <= 0) 
        {
            ReturnPool.ReturnPool(this);
        }
    }
    
}
