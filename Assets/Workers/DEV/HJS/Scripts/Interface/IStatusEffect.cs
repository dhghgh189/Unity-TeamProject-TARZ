using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;
/// <summary>
/// 이거 있으면 상태이상을 받을 수 있게 해주는 인터페이스,
/// ex) 기절, 공격 약해짐, 빙결, 전기, etc
/// </summary>
public interface IStatusEffect
{
    /// <summary>
    /// 슬로우 효과
    /// </summary>
    /// <param name="attacker">요청한(공격한) 오브젝트</param>
    /// <param name="target">효과를 받는 오브젝트</param>
    /// <param name="degree">느려지는 정도</param>
    /// <param name="duration">지속 시간</param>
    public void SlowEffect(GameObject attacker, GameObject target, float degree, float duration);
    /// <summary>
    /// 전기 효과
    /// </summary>
    /// <param name="attacker">요청한(공격한) 오브젝트</param>
    /// <param name="target">효과를 받는 오브젝트</param>
    /// <param name="dotDamage">도트 데미지의 량</param>
    /// <param name="duration">지속 시간</param>
    public void ElectroEffect(GameObject attacker, GameObject target, float dotDamage, float duration);
    /// <summary>
    /// 독 중독 효과
    /// </summary>
    /// <param name="attacker">요청한(공격한) 오브젝트</param>
    /// <param name="target">효과를 받는 오브젝트</param>
    /// <param name="dotDamage">도트 데미지의 량</param>
    /// <param name="duration">지속 시간</param>
    public void PoisonEffect(GameObject attacker, GameObject target, float dotDamage, float duration);
    /// <summary>
    /// 빙결 효과
    /// </summary>
    /// <param name="attacker">요청한(공격한) 오브젝트</param>
    /// <param name="target">효과를 받는 오브젝트</param>
    /// <param name="duration">지속 시간</param>
    public void FrozenEffect(GameObject attacker, GameObject target, float duration);

}
