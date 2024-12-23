using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        if (_monsterData.CurHp <= 0) //업데이트가 아니라 이벤트로 해줘도 좋을듯!ㄴ
        {
            ReturnPool.ReturnPool(this);
        }
    }
}
