using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_Revive : Action
{
    [Inject]
    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _reviveBefore;

    [SerializeField] GameObject _reviveAfter;

    public override void OnStart()
    {
        _monsterSkillManager.ReviveAfter = _reviveAfter;
        _monsterSkillManager.ReviveBefore = _reviveBefore;
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterData.CurHp <= _monsterData.MaxHp / 2 && _monsterSkillManager.ReviveSkill.CanUseSkill)
        {
            _monsterSkillManager.Revive();
            Debug.Log("부활");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
