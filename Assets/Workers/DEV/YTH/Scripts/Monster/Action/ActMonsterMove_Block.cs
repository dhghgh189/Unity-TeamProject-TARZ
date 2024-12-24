using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class ActMonsterMove_Block : Action
{
    [SerializeField] CondMonsterCanMove _condMonsterCanMove;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Animator _animator;

    [SerializeField] GameObject _player;

    [SerializeField] Vector3 _playerBackRoute;

    [SerializeField] float _stopBlockDistance = 5f; // 공격 거리 보다 조금 멀게

    private Transform _lastPlayerTransform; // 플레이어가 시야각에서 사라진 마지막 위치

    private float _distance;

    public override void OnStart()
    {
        getLasPlayerTransform = StartCoroutine(GetLasPlayerTransform());
    }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        _playerBackRoute = _player.transform.position - _player.transform.forward * 10f;

        if (_condMonsterCanMove.ReturnObj != null && _distance > _stopBlockDistance) // _condMonsterCanMove.ReturnObj 는 시야각 내의 물체 (플레이어)
        {
            _agent.SetDestination(_playerBackRoute);
            return TaskStatus.Running;
        }
        else if (_condMonsterCanMove.ReturnObj != null && _distance <= _stopBlockDistance)
        {
            if (_condMonsterCanMove.ReturnObj != null && _distance <= _monsterData.AttackRange)
            {
                return TaskStatus.Success;
            }
            _agent.SetDestination(_player.transform.position);
            return TaskStatus.Running;
        }

        else if (_condMonsterCanMove.ReturnObj == null)
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
    Coroutine getLasPlayerTransform;
    IEnumerator GetLasPlayerTransform()
    {
        if (_condMonsterCanMove.ReturnObj == null)
        {
            _lastPlayerTransform = _player.transform;
        }
        yield return null;
        getLasPlayerTransform = null;
    }
}


