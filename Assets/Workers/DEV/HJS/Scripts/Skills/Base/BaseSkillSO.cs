using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SkillEnum;

[CreateAssetMenu(menuName = "Scriptables/Base_Skill")]
public class BaseSkillSO : ScriptableObject
{
    [Header("Skill_Info")]
    public string Name;                   // 스킬 이름
    public string Description;            // 스킬 설명
    public Sprite Icon;                   // 스킬 아이콘
    [Range(1, 3)] public int MaxLevel;    // 레벨업 가능한 최대 레벨
    private int skillLevel;
    [Space]
    [Header("ActTiming")]
    public ActTimingType Timing;  // 타이밍
    [Space(3)]
    [Header("Effect_Active")]
    [Space(2)]
    public List<ActiveSkill> activeSkills;
    [Space(2)]
    [Header("Effect_Passive")]
    public List<PassiveSkill> passiveSkills;
    public int SkillLevel { get { return skillLevel; } set { skillLevel = value; onChangeLevel?.Invoke(skillLevel); } }

    [HideInInspector]
    public UnityEvent<int> onChangeLevel;
    [HideInInspector]
    public StatModel model;

    private void OnDestroy()
    {
        onChangeLevel.RemoveAllListeners();
        Debug.Log($"스킬 {name}가 파괴되었습니다");
    }
}

