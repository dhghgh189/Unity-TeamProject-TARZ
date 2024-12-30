using BehaviorDesigner.Runtime.Tasks;
using static MonsterData;
using System.Collections;
using UnityEngine;

public class CondCanSkill : Conditional
{
    [SerializeField] MonsterData _monsterData;

    public override TaskStatus OnUpdate()
    {
        if (_monsterData.SkillTyPe == SkillType.Skill)
        {
            Debug.Log("CondMonsterCanSkill@@@@@@");
            return TaskStatus.Success;
        }
        else
        {
            Debug.Log("CondMonsterCanSkillXXXXXXXXXXXX");
            return TaskStatus.Failure;
        }
    }
    
}
