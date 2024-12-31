using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;

public class MSkill_StimPack : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private MonsterData _monsterData;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _monsterData = GetComponent<MonsterData>();
    }

    public override TaskStatus OnUpdate()
	{
        if (_monsterData.CurHp <= 0)
        {
            _monsterSkillManager.StimPak();
            Debug.Log("스팀팩");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}