using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

[CreateAssetMenu(menuName = "Scriptables/Base_PassiveSkill")]
public class PassiveSkillSO : ScriptableObject
{
    [SerializeField] PassiveType passiveType;

    [Space(2)]
    [Header("Settings")]
    [SerializeField, ShowEnum((int)PassiveType.Modify, "passiveType")] ModifySetting modifySetting;
    [SerializeField, ShowEnum((int)PassiveType.Condition, "passiveType")] ConditionSetting conditionSetting;

    // Modify - 수정
    // 값의 수정을 담당
    // 해당 적용을 할 때 바로 state에게 적용할 거 같다
    // 대폭 이렇게 있지만 -> 공격력을 더해준다
    [Serializable]
    public class ModifySetting
    {
        // Stat의 값 조절
        // 플레이어의 기존 범위
        // 이건 기획팀에서의 필요한  스텟에 따라 달라질거 같다
    }

    // Condition - 조건
    // 값의 변경에 따라 행동 담당
    // 해당 내용은 MVC 에서 -> Model의 이벤트에 연결해서 사용할 거 같다.
    // 함수의 내용은 따로 정의 해야할 거 같다
    [Serializable]
    public class ConditionSetting
    {

    }
}
