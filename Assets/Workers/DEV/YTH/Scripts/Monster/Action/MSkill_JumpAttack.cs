using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MSkill_JumpAttack : Action
{
    private MonsterSkillManager _monsterSkillManager;

    [Inject] private PlayerController _player;

    private float _distance;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
    }

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance >= 40 && _monsterSkillManager.JumpAttackSkill.CanUseSkill == true && _monsterSkillManager.jumpAttackRoutine == null)
        {
            _monsterSkillManager.jumpAttackRoutine = StartCoroutine( _monsterSkillManager.JumpAttackRoutine());
            Debug.Log("점프어택");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
