using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MSkill_JumpAttack : Action
{
    [Inject]
    private CoroutineManager _util;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _player;

    private float _distance;

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance >= 40 && _monsterSkillManager.JumpAttackSkill.CanUseSkill == true)
        {
            _util.StartRoutine(ref _monsterSkillManager.jumpAttackRoutine, _monsterSkillManager.JumpAttackRoutine());
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
