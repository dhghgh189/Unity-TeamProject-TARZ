using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
public class ActMove_Back : Action
{
    [SerializeField] CondCanMove _condCanMove;

    private PooledObject _pooledObject;

    private MonsterData _monsterData;

    private NavMeshAgent _agent;

    private Animator _animator;

    private PlayerController _player; 

    private Vector3 _back;

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
}

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        _back = transform.position - transform.forward * 2f;

        if (_condCanMove.IsPlayerWithinSight(_player.gameObject) && _distance < _monsterData.DangerDistance)
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


