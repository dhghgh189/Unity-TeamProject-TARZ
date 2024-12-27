using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_Thunder : Action
{
    [Inject]
    private CoroutineManager _util;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _player;

    private float _distance;

    public override void OnStart()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance < 20 && _monsterSkillManager.ThunderSkill.CanUseSkill == true && _monsterData.CurHp <= _monsterData.MaxHp / 2)
        {
            _util.StartRoutine(ref _monsterSkillManager.thunderRoutine, _monsterSkillManager.ThunderRoutine());
            Debug.Log("10 ThunderRoutine 시작");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
        
    }
}
