using BehaviorDesigner.Runtime.Tasks;
using static MonsterData;
using System.Collections;
using UnityEngine;

public class CondCanSkill : Conditional
{
    private MonsterData _monsterData;

    public override void OnAwake()
    {
        _monsterData = GetComponent<MonsterData>();
    }

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
