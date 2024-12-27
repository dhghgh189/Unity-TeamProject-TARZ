using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_TrippleAttack : Action
{
    [Inject]
    private CoroutineManager _util;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _player;

    private float _distance;

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.TrippleAttackSkill.CanUseSkill == true && _distance <= 10)
        {
            _util.StartRoutine(ref _monsterSkillManager.trippleAttackRoutine, _monsterSkillManager.TrippleAttackRoutine());
            Debug.Log("trippleAttack");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}