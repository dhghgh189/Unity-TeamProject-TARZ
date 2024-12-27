using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마나 스킬인지 확인하는 여부
/// </summary>
public interface IManaSkill
{
    /// <summary>
    /// 스킬의 데이터
    /// </summary>
    public ManaSkillDataSO SkillData { get; set; }

    /// <summary>
    /// 스킬을 시작할 부분
    /// </summary>
    public void SetInit(ManaSkillHandler manaSkillHandler);

}
