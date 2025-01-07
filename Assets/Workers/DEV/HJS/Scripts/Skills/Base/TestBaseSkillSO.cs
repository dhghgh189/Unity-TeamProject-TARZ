using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEditor;

[Flags]
public enum Test_Skill { Attack = 1 << 0, Dash = 1 << 1 }
[Flags]
public enum Test_Type { None = 0, Act = 1 << 0, Etc = 1 << 1 }
[CreateAssetMenu(menuName = "Scriptables/Test_Base_Skill")]
public class TestBaseSkillSO : ScriptableObject
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
    private int skillLevel;               // 스킬의 현재 레벨
    [Space(3)]
    // 여기서 타입 결정
    // 무슨타입? 플레이어의 행동이나 아니면 그 외 모든 것이냐
    [Header("스킬 종류")]
    public Test_Type skillType;
    [ShowFlags((int)Test_Type.Act, "skillType")]
    [Tooltip("스킬이 발동할 수 있는 행동 조건")] public Test_Skill skill;              // 플레이어의 행동 조건
    [ShowFlags((int)Test_Type.Act, "skillType")]
    [SerializeField] ActiveSkills active;           // 행동 스킬

    [ShowFlags((int)Test_Type.Etc, "skillType")]
    [SerializeField] PassiveSkills passive;         // 그외 스킬들

    [HideInInspector]
    public UnityEvent<int> onChangeLevel; // 스킬의 레벨이 업데이트를 알려주는 이벤트
    [HideInInspector]
    public StatModel model;               // 정보가 들어있는 함수

    public List<ActiveSkillSO> ActiveSkills => active.activeSkillSOs;
    public List<PassiveSkillSO> PassiveSkills => passive.passiveSkillSOs;

    public bool CheckAct(Test_Skill curAct)
    {
        // 포함이 되어있는지 확인
        return (curAct & skill) != 0;
    }
}
