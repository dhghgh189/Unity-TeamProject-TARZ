using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

/// <summary>
/// 1. 플레이어 뒤로 가다가 stopblockDistance보다 클때 running 
/// 2. stopBlockDistance보다 가까워지면 추격 으로 넘어가게 Running 
/// </summary>
public class ActMove_Block : Action
{
    [SerializeField] CondCanMove _condCanMove;
    private PooledObject _pooledObject;

    private MonsterData _monsterData;
    private NavMeshAgent _agent;
    private Animator _animator;

    private PlayerController _player;

    private Vector3 _playerBackRoute;

    [SerializeField] float _stopBlockDistance = 5f; // 공격 거리 보다 조금 멀게

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

        _playerBackRoute = _player.transform.position - _player.transform.forward * 10f;

        if (_condCanMove.IsPlayerWithinSight(_player.gameObject) && _distance >= _stopBlockDistance) 
        {
            _agent.SetDestination(_playerBackRoute);
            return TaskStatus.Running;
        }
        else if (_condCanMove.IsPlayerWithinSight(_player.gameObject) && _distance < _stopBlockDistance)
        {
            if (_condCanMove.IsPlayerWithinSight(_player.gameObject) && _distance <= _monsterData.AttackRange)
            {
                return TaskStatus.Success;
            }
            _agent.SetDestination(_player.transform.position);
            return TaskStatus.Running;
        }

        else if (_condCanMove.IsPlayerWithinSight(_player.gameObject))
        {
            //_agent.SetDestination(_lastPlayerTransform.position);
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


