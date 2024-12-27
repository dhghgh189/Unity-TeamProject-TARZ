using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MSkill_FrogJuMP : Action
{
    [Inject]
    private CoroutineManager _util;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    private float _distance;
    public override TaskStatus OnUpdate()
    {
        if (_distance > _monsterData.CanJumpDistance)
        {
            _util.StartRoutine(ref _monsterSkillManager.frogJumpAttackRoutine, _monsterSkillManager.FrogJumpAttackRoutine());
            Debug.Log("개구리 점프!!");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
