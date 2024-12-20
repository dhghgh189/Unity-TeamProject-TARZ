using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬의 정보 업데이트가 가능하게 해주는 인터페이스
/// </summary>
public interface ISpec
{
    /// <summary>
    /// 스킬의 정보를 업데이트 해주는 함수,
    /// 레벨에 해당하는 정보를 준다
    /// </summary>
    /// <param name="spec">정보</param>
    /// <param name="level">레벨</param>
    public void SetSpec(SkillSpecDatabase.Spec spec, int level);
}
