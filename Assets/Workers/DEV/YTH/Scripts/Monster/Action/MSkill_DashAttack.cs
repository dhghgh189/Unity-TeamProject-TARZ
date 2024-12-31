using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_DashAttack : Action
{
    private MonsterSkillManager _monsterSkillManager;

    [Inject] private PlayerController _player;

    private float _distance;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
    }

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if ( _distance < 30 && _monsterSkillManager.DashAttackSkill.CanUseSkill == true && _monsterSkillManager.dashAttackRoutine == null)
        {
            _monsterSkillManager.dashAttackRoutine = StartCoroutine(_monsterSkillManager.DashAttackRoutine());
            Debug.Log("20 DashAttackRoutine");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}