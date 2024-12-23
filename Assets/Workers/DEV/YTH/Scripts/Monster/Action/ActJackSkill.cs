using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ActJackSkill : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] float _distance;

    [SerializeField] GameObject _player;

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance <= 10)
        {

            if (_monsterSkillManager.TrippleAttackSkill.CanUseSkill == true)
            {
                _monsterSkillManager.trippleAttackRoutine = StartCoroutine(_monsterSkillManager.TrippleAttackRoutine());
                Debug.Log("trippleAttack");
                return TaskStatus.Success;
            }
            else if (_monsterSkillManager.WheelWindSkill.CanUseSkill == true && _monsterData.CurHp <= _monsterData.MaxHp / 2)
            {
                _monsterSkillManager.wheelWindRoutine = StartCoroutine(_monsterSkillManager.WheelWindRoutine());
                Debug.Log("wheelWind");
                return TaskStatus.Success;
            }


            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
