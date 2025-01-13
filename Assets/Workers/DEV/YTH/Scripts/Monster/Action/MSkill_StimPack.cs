using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

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
        if (_monsterData.CurHp <= 0 && _monsterSkillManager.JackTheRipper != null)
        {
            if (_monsterSkillManager.stimPakRoutine == null)
            {
                _monsterSkillManager.stimPakRoutine = StartCoroutine(_monsterSkillManager.StimPak());
                SoundManager.PlaySFX(SoundManager.SoundData_M.StimPak);
                Debug.Log("스팀팩");
            }
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}