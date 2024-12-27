using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 1. returnObj로 setdastination  running + 공격 거리 내로 들어오면 succeess
/// 2. 시야에서 놓치면 마지막 위치까지 가게 succeess 
/// </summary>
public class ActMove_NotStopInAttacking : Action
{
    [SerializeField] CondCanMove _condCanMove;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Animator _animator;

    [SerializeField] GameObject _player; 

    private Transform _lastPlayerTransform; // 플레이어가 시야각에서 사라진 마지막 위치

    private float _distance;

    public override void OnStart()
    {
        keepChaseRoutine = StartCoroutine(KeepChaseRoutine());
    }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_condCanMove.IsPlayerWithinSight(_player)/* && !_monsterData.IsAttacked*/)
        {
            if( _distance <= _monsterData.AttackRange)
            {
                return TaskStatus.Success;
            }

            _agent.SetDestination(_player.transform.position);
            return TaskStatus.Running;
            // _animator.SetBool("walk", true); // 해쉬로 바꿔주면 좋을듯
        }
        else if (_condCanMove.IsPlayerWithinSight(_player))
        {
            _agent.SetDestination(_lastPlayerTransform.position);
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }


    /// <summary>
    /// 플레이어가 시야에서 사라졌을때 마지막 플레이어 위치 기억
    /// </summary>
    Coroutine keepChaseRoutine;
    IEnumerator KeepChaseRoutine()
    {
        if (_condCanMove.IsPlayerWithinSight(_player) == false)
        {
            _lastPlayerTransform = _player.transform;
        }
        yield return null;
        keepChaseRoutine = null;
    }
}


