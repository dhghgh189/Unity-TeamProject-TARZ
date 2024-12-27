using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;

public class MSkill_StimPack : Action
{
    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] MonsterData _monsterData;


    public override TaskStatus OnUpdate()
	{
        if (_monsterData.CurHp <= 0)
        {
            _monsterSkillManager.StimPak();
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}