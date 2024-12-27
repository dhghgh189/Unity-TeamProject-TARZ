using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class ActMove_Back : Action
{
    [SerializeField] CondCanMove _condCanMove;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Animator _animator;

    [SerializeField] GameObject _player; 

    private Vector3 _back;

    private float _distance;
   
    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        _back = transform.position - transform.forward * 2f;

        if (_condCanMove.IsPlayerWithinSight(_player) && _distance < _monsterData.DangerDistance)
        {
            if (_distance <= _monsterData.AttackRange || _distance <= _monsterData.CanUseProjectileSkillDistance)
            {
                _agent.isStopped = true;

                return TaskStatus.Success;
            }

            _agent.isStopped = false;
            _agent.SetDestination(_back);
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}


