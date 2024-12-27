using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_WheelWind : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _wheelWindTrigger;

    [SerializeField] GameObject _player;

    private float _distance;

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        _monsterSkillManager.WheelWindTrigger = _wheelWindTrigger;
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.WheelWindSkill.CanUseSkill == true && _monsterData.CurHp <= _monsterData.MaxHp / 2 && _distance <= 10 && _monsterSkillManager.wheelWindRoutine == null)
        {
            _monsterSkillManager.wheelWindRoutine = StartCoroutine(_monsterSkillManager.WheelWindRoutine());
            Debug.Log("wheelWind");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}


