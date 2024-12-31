using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_TrippleAttack : Action
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
        if (_monsterSkillManager.TrippleAttackSkill.CanUseSkill == true && _distance <= 10 && _monsterSkillManager.trippleAttackRoutine == null)
        {
            _monsterSkillManager.trippleAttackRoutine = StartCoroutine(_monsterSkillManager.TrippleAttackRoutine());
            Debug.Log("trippleAttack");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}