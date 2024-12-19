using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class ActMonsterMove : Action
{
    [SerializeField] CondMonsterCanMove _condMonsterCanMove;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Animator _animator;

    [SerializeField] GameObject _player; 

    private Transform _lastPlayerTransform; // 플레이어가 시야각에서 사라진 마지막 위치
   
    public override void OnStart()
    {
        getLasPlayerTransform = StartCoroutine(GetLasPlayerTransform());
    }

    public override TaskStatus OnUpdate()
    {
        if (_condMonsterCanMove.ReturnObj != null && !_monsterData.IsAttacked) // _condMonsterCanMove.ReturnObj 는 시야각 내의 물체 (플레이어)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) <= _monsterData.AttackRange || _monsterData.CanUseSkill ==true)
            {
                return TaskStatus.Success;
            }
            _agent.SetDestination(_condMonsterCanMove.ReturnObj.transform.position);
            return TaskStatus.Running;
            // _animator.SetBool("walk", true); // 해쉬로 바꿔주면 좋을듯
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


