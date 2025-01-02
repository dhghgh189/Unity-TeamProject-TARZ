using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MSkill_FrogJump : Action
{
    private MonsterSkillManager _monsterSkillManager;
    private Animator _animator;
    private MonsterData _monsterData;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _animator = GetComponent<Animator>();
        _monsterData = GetComponent<MonsterData>();
    }

    private float _distance;
    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.frogJumpAttackRoutine == null)
        {
            _monsterSkillManager.frogJumpAttackRoutine = StartCoroutine(_monsterSkillManager.FrogJumpAttackRoutine());
            _animator.SetTrigger("Jump");
            Debug.Log("개구리 점프!!");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
