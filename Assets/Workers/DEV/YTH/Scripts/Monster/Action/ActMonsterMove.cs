using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
public class ActMonsterMove : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Animator _animator;

    [SerializeField] GameObject target;
    public override TaskStatus OnUpdate()
    {

        if (Vector3.Distance(transform.position, target.transform.position) < _monsterData.TraceRange && !_monsterData.IsAttacked)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= _monsterData.AttackRange /*&& !_monsterData.IsAttacked*/)
            {
                return TaskStatus.Success;
            }
            _agent.SetDestination(target.transform.position);
            transform.LookAt(target.transform);
            // _animator.SetBool("walk", true); // 해쉬로 바꿔주면 좋을듯
            Debug.Log("이동중");
            return TaskStatus.Running;
        }
        else
        {
            _agent.isStopped = true;
            // _animator.SetBool("Idle", true);
            return TaskStatus.Failure;
        }
    }
}


