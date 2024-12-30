using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MSkill_FrogJump : Action
{
    [Inject]
    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] MonsterData _monsterData;

    private float _distance;
    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.frogJumpAttackRoutine == null)
        {
            _monsterSkillManager.frogJumpAttackRoutine = StartCoroutine(_monsterSkillManager.FrogJumpAttackRoutine());
            Debug.Log("개구리 점프!!");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
