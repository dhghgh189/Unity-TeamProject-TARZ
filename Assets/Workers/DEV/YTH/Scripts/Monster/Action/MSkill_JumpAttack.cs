using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MSkill_JumpAttack : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private PooledObject _pooledObject;

    private PlayerController _player;

    private float _distance;

    private Animator _animator;

    public override void OnAwake()
    {
        _animator = GetComponent<Animator>();
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _pooledObject = GetComponent<PooledObject>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance >= 15 && _monsterSkillManager.JumpAttackSkill.CanUseSkill == true && _monsterSkillManager.jumpAttackRoutine == null)
        {
            _pooledObject.RotateToPlayer();
            _monsterSkillManager.jumpAttackRoutine = StartCoroutine( _monsterSkillManager.JumpAttackRoutine());
            _animator.SetTrigger("JumpAttack");
            SoundManager.PlaySFX(SoundManager.SoundData_M.JumpAttack);

            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
