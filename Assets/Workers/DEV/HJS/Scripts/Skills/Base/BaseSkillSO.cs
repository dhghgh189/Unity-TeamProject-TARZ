using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SkillEnum;

[CreateAssetMenu(menuName = "Scriptables/Base_Skill")]
public class BaseSkillSO : ScriptableObject
{
    [Header("Skill_Info")]
    public new string Name;               // 스킬 이름
    public string Description;            // 스킬 설명
    public Sprite Icon;                   // 스킬 아이콘
    [Space]
    [Header("ActTiming")]
    public ActTimingType Timing;  // 타이밍
    [Space(2)]
    [Header("Effect_Active")]
    public List<ActiveSkillSO> EffectSkills;
    [Space(2)]
    [Header("Effect_Passive")]
    public List<PassiveSkillSO> PassiveSkills;

}
