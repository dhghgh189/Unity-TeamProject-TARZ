using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class ActAnoldSkill : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] float _distance;

    [SerializeField] GameObject _target;

    [SerializeField] NavMeshAgent _agent;

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _target.transform.position);
       /* _agent.enabled = false;*/
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance >= 40 &&  _monsterSkillManager.JumpAttackSkill.CanUseSkill == true)
        {
            if (_monsterSkillManager.jumpAttackRoutine == null)
            {
                _monsterSkillManager.jumpAttackRoutine =  StartCoroutine(_monsterSkillManager.JumpAttackRoutine());
                Debug.Log("40 JumpAttack");
            }
            return TaskStatus.Success;
        }
        /*else if (_distance >= 30 && _distance < 40 && _monsterSkillManager.ElectricWallSkill.CanUseSkill == true)
        {
            if (_monsterSkillManager.jumpAttackRoutine == null) // 일레트릭월로 변경해야댐
            {
                //ElectricWall
                Debug.Log("30 ElectricWall");
            }
            return TaskStatus.Success;
        }*/
        else if (_distance >= 20 && _distance < 30 && _monsterSkillManager.DashAttackSkill.CanUseSkill == true)
        {
            if (_monsterSkillManager.dashAttackRoutine == null)
            {
                _monsterSkillManager.dashAttackRoutine = StartCoroutine(_monsterSkillManager.DashAttackRoutine());
                Debug.Log("20 DashAttackRoutine");
            }
            return TaskStatus.Success;
        }
        else if (_distance < 20)  // 다가가 공격으로 넘어감
        {
            return TaskStatus.Success;
        }
        else
        {
            return  TaskStatus.Failure;
        }
    }

    /* public override void OnEnd()
     {
         _agent.enabled = true;
     }*/
}
