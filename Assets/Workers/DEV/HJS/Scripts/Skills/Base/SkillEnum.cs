using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEnum : MonoBehaviour
{
    /// <summary>
    /// 스킬을 발동 타이밍
    /// </summary>
    public enum ActTimingType { Attack, Drain, Dash, None }
    /// <summary>
    /// 스킬이 작동하는 시점
    /// </summary>
    public enum ActConditionType { Start, Collision }
    /// <summary>
    /// 누가 스킬의 주최가 될건지
    /// </summary>
    public enum Target { Player, ThrowObject }
    /// <summary>
    /// 해당 능력의 사용될 빈도수
    /// </summary>
    public enum RefeatType { Always, Once }
    /// <summary>
    /// 패시브 스킬의 종류
    /// </summary>
    public enum PassiveType { Modify, Condition }
}
