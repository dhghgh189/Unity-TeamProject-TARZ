using BehaviorDesigner.Runtime.Tasks;
using static MonsterData;
using System.Collections;
using UnityEngine;

public class CondMonsterCanSkill : Conditional
{
    [SerializeField] MonsterData _monsterData;

    public override TaskStatus OnUpdate()
    {
        if (_monsterData.Type == MonsterType.Boss && _monsterData.UseSkill == false)
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
