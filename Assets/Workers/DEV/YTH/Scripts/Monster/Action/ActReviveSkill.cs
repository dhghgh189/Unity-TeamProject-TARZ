using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class ActReviveSkill : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    public override TaskStatus OnUpdate()
    {
        if (_monsterData.CurHp <= 0 && _monsterSkillManager.ReviveSkill.CanUseSkill == true)
        {
            if (_monsterSkillManager.reviveRoutine == null)
            {
                _monsterSkillManager.reviveRoutine = StartCoroutine(_monsterSkillManager.ReviveRoutine());
                Debug.Log("부활");
            }
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
