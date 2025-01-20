using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CondCanAttack : Conditional
{
    private MonsterData _monsterData;

    private PlayerController _player;

    private float _distance;

    private PooledObject _pooledObject;

    public override void OnAwake()
    {
        _monsterData = GetComponent<MonsterData>();
        _pooledObject = GetComponent<PooledObject>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;

        //Debug.Log("컨디션 캔 어택 3 ");

    }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_distance <= _monsterData.AttackRange && !_monsterData.IsAttacked && !_monsterData.IsCatched && _monsterData.IsDead == false)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    /*private bool IsAvailable()
    {
        if (_distance <= _monsterData.AttackRange
             && !_monsterData.IsAttacked
             && !_monsterData.IsCatched
             && _monsterData.IsDead == false)
        {
            return true;
        }
        return false;
    }*/
}
