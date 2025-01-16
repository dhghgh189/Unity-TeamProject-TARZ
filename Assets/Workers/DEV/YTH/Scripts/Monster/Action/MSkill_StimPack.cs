using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;

public class MSkill_StimPack : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private MonsterData _monsterData;

    private GameObject _bomber;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _monsterData = GetComponent<MonsterData>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.StimPakSkill.CanUseSkill && _monsterData.CurHp <= _monsterData.MaxHp/3)
        {
            _monsterSkillManager.stimPakRoutine = StartCoroutine(_monsterSkillManager.StimPak());
            SoundManager.PlaySFX(SoundManager.SoundData_M.StimPak_Bomber);
            SoundManager.PlaySFX(SoundManager.SoundData_M.StimPak_Jack);
            Debug.Log("스팀팩");

            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}