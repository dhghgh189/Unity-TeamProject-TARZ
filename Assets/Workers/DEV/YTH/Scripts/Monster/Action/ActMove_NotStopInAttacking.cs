using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

/// <summary>
/// 1. returnObj로 setdastination  running + 공격 거리 내로 들어오면 succeess
/// 2. 시야에서 놓치면 마지막 위치까지 가게 succeess 
/// </summary>
public class ActMove_NotStopInAttacking : Action
{
    [SerializeField] CondCanMove _condCanMove;
    private PooledObject _pooledObject;

    private MonsterData _monsterData;
    private NavMeshAgent _agent;
    private Animator _animator;

    private PlayerController _player;

    private Transform _lastPlayerTransform; // 플레이어가 시야각에서 사라진 마지막 위치

    private float _distance;

    public override void OnAwake()
    {
        _pooledObject = GetComponent<PooledObject>();
        _monsterData = GetComponent<MonsterData>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
        keepChaseRoutine = StartCoroutine(KeepChaseRoutine());
    }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_condCanMove.IsPlayerWithinSight(_player.gameObject) && !_monsterData.IsAttacked)
        {
            if (_distance <= _monsterData.AttackRange || _distance <= _monsterData.CanJumpDistance)
            {
                return TaskStatus.Success;
            }

            _agent.SetDestination(_player.transform.position);
            _animator.SetBool("Move", true);
            return TaskStatus.Running;
        }
        else if (_condCanMove.IsPlayerWithinSight(_player.gameObject) == false)
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
        if (_condCanMove.IsPlayerWithinSight(_player.gameObject) == false)
        {
            _lastPlayerTransform = _player.transform;
        }
        yield return null;
        keepChaseRoutine = null;
    }
}


