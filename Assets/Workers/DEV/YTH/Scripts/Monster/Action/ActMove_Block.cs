using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ActMove_Block : Action
{
    [SerializeField] CondCanMove _condCanMove;

    private float _stopBlockDistance;
    public float StopBlockDistance { get { return _stopBlockDistance; } set { _stopBlockDistance = value; } }

    private PooledObject _pooledObject;

    private MonsterData _monsterData;

    private NavMeshAgent _agent;

    private Animator _animator;

    private PlayerController _player;

    private Vector3 _playerBackRoute;

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

        _stopBlockDistance = _monsterData.CanJumpDistance + 2;

        Debug.Log("액트 무브 _ 블락 : 2");
    }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        _playerBackRoute = _player.transform.position - _player.transform.forward * _stopBlockDistance;

        // 캐릭터의 뒤로 걸어서 간다
        // 마주보고 있으면 캐릭터 뒤 좌표로 직진 => 자연스럽게 거리가 가까워지면 캐릭터를 추격하게ㅐ
        // 멀리있을때는 캐릭터의 뒤로 갈 수있음
        // 하지만 캐릭터를 추적하는 본질은 유지
        if (_condCanMove.IsPlayerWithinSight(_player.gameObject) && _distance > _stopBlockDistance)
        {
            _agent.SetDestination(_playerBackRoute);
            _animator.SetBool("Move", true);

            return TaskStatus.Running;
        }
        else if (_condCanMove.IsPlayerWithinSight(_player.gameObject) && _distance <= _stopBlockDistance + 1)
        {
            if ( _distance <= _monsterData.CanJumpDistance || _distance <= _monsterData.AttackRange )
            {
                return TaskStatus.Success;
            }

            _agent.SetDestination(_player.transform.position);
            return TaskStatus.Running;
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


