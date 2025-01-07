using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SkillEnum;

[CreateAssetMenu(menuName = "Scriptables/Base_Skill")]
public class BaseSkillSO : ScriptableObject
{
    [Header("스킬 기본 정보")]
    [Header("스킬 이름")]
    public string Name;                   // 스킬 이름
    [Header("스킬 설명")]
    public string Description;            // 스킬 설명
    [Header("스킬 아이콘")]
    public Sprite Icon;                   // 스킬 아이콘
    [Header("최대 레벨")]
    [Range(1, 3)] public int MaxLevel;    // 레벨업 가능한 최대 레벨
    private int skillLevel;                         // 스킬의 현재 레벨
    [Header("스킬 티어")]
    [Range(1, 3)] public int SkillTier;   // 스킬 티어  
    [Space(3)]
    [Header("스킬 종류")]
    public SkillType SkillType;
#if UNITY_EDITOR
    [ShowFlags((int)SkillType.Act, "SkillType")]
#endif
    [SerializeField] ActiveSkills active;           // 행동 스킬

#if UNITY_EDITOR
    [ShowFlags((int)SkillType.Etc, "SkillType")]
#endif
    [SerializeField] PassiveSkills passive;         // 그외 스킬들

    public int SkillLevel { get { return skillLevel; } set { skillLevel = value; onChangeLevel?.Invoke(skillLevel); } }

    [HideInInspector]
    public UnityEvent<int> onChangeLevel; // 스킬의 레벨이 업데이트를 알려주는 이벤트

    public List<ActiveSkill> ActiveSkills => active.activeSkills;
    public List<PassiveSkill> PassiveSkills => passive.passiveSkills;


    private void OnDestroy()
    {
        onChangeLevel.RemoveAllListeners();
        Debug.Log($"스킬 {name}가 파괴되었습니다");
    }
}

