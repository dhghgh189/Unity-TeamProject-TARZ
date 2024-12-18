using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static MonsterData;

public class ActMonsterSkill : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] float _distance;

    [SerializeField] GameObject _target;

    [SerializeField] NavMeshAgent _agent;

    public override void OnStart()
    {
        _agent.enabled = false;
    }

    public override TaskStatus OnUpdate()
    {
        // �̰� ���ľߴ�
        // ��ų ��� ��
        // �Ϲݸ��� �����۵��ϴµ�
        // ��ų���� �� ��� ���� �����ϰ� ���� �����ؾߴ�

        _distance = Vector3.Distance(transform.position, _target.transform.position);

        if (_monsterData.Type == MonsterType.Boss /*&& ������ ��ų ��� ������ ����*/)
        {
            if (_distance >= 40)
            {
                if (_monsterSkillManager.jumpAttackRoutine == null)
                {
                    StartCoroutine(_monsterSkillManager.JumpAttackRoutine());
                    Debug.Log("40");
                }
                return TaskStatus.Success;
            }
            else if (_distance >= 30 && _distance < 40)
            {
                if (_monsterSkillManager.jumpAttackRoutine == null)
                {
                    StartCoroutine(_monsterSkillManager.JumpAttackRoutine());
                    Debug.Log("30");
                }
                return TaskStatus.Success;
            }
            else if (_distance >= 20 && _distance < 30)
            {
                if (_monsterSkillManager.jumpAttackRoutine == null)
                {
                    StartCoroutine(_monsterSkillManager.JumpAttackRoutine());
                    Debug.Log("20");
                }
                return TaskStatus.Success;
            }
            else if (_distance >= 10 && _distance < 20)
            {
                if (_monsterSkillManager.jumpAttackRoutine == null)
                {
                    StartCoroutine(_monsterSkillManager.JumpAttackRoutine());
                    Debug.Log("10");
                }
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        else
        {
            Debug.Log("�� �����ƴ϶� ��ų ����");
            return TaskStatus.Failure;
        }
    }

    public override void OnEnd()
    {
        _agent.enabled = true;
    }
}
