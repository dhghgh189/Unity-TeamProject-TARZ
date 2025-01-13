using BehaviorDesigner.Runtime.Tasks;
using static MonsterData;
using System.Collections;
using UnityEngine;

public class CondCanSkill : Conditional
{
    [SerializeField] CondCanMove _condCanMove;

    private MonsterData _monsterData;

    private PlayerController _player;

    private PooledObject _pooledObject;

    public override void OnAwake()
    {
        _monsterData = GetComponent<MonsterData>();
        _pooledObject = GetComponent<PooledObject>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterData.SkillTyPe == SkillType.Skill && _condCanMove.IsPlayerWithinSight(_player.gameObject) && !_monsterData.IsAttacked && _monsterData.IsDead == false)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
