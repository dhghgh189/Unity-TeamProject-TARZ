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
    public enum PassiveType { Modify, Condition, Toggle }
    /// <summary>
    /// 상호작용의 종류
    /// </summary>
    public enum InteractionType { Slow, Elec, Frozen, Poison, Damage, SIZE }
    /// <summary>
    /// 패시브 - 값의 종류
    /// </summary>
    public enum PassiveModifyType { DashTime = -2, DrainRadius = -1, MaxHp = 0, MaxStamina, MoveSpeed, AllPower, DefaultPower, StaminaCostRate }
    /// <summary>
    /// 패시브 - 입력 값의 종류
    /// </summary>
    public enum PassiveModifyInputType { Value, Percent }
    /// <summary>
    /// 패시브 - 결과 값의 종류
    /// </summary>
    public enum PassiveResultModifyType { MaxHp = 0, MaxStamina, MoveSpeed, AllPower, DefaultPower, StaminaCostRate }
    /// <summary>
    /// 패시브 - 조건의 종류
    /// </summary>
    public enum ConditionType { Greater, Less, GreaterEqual, LessEqual, Equal, NotEqual }
    /// <summary>
    /// 패시브 - 활성화/비활성화
    /// </summary>
    public enum ToggleType { Collision, Function }
}
