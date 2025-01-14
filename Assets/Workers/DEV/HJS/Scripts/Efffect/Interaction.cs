using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static SkillEnum;

/// <summary>
/// 상호작용을 할 수 있게 해주는 클래스
/// </summary>
public class Interaction : ISpec
{
    /// <summary>
    /// 해당 스킬의 타입 프토퍼티
    /// </summary>
    public InteractionType Type { get; set; }

    /// <summary>
    /// 디버프의 정도
    /// </summary>
    private float degree;
    /// <summary>
    /// 입힐 데미지의 양
    /// </summary>
    private float dotDamage;
    /// <summary>
    /// 지속 시간
    /// </summary>
    private float duration;

    public Interaction(InteractionType type)
    {
        this.Type = type;
        degree = 0f;
        dotDamage = 0f;
        duration = 0f;
    }

    public void Activate(GameObject attacker, GameObject target)
    {
        Debug.Log("<color=red>Activate Abnormal status</color>");
        Test_StatusEffect statusEffectable = target.GetComponentInChildren<Test_StatusEffect>();
        if (statusEffectable == null) return;

        switch (Type)
        {
            case InteractionType.Slow:
                // Slow 적용하기
                statusEffectable.SlowSkill(degree, duration);
                break;
            case InteractionType.DOT:
                // TODO: 지속딜 넣기
                break;
            case InteractionType.Damage:
                IDamagable damagable = target.GetComponent<IDamagable>();
                damagable?.TakeDamage((int)dotDamage);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 스킬의 능력치를 설정해주는 함수
    /// </summary>
    /// <param name="spec">스킬의 능력치</param>
    /// <param name="level">스킬의 레벨</param>
    public void SetSpec(Spec spec, int level)
    {
        degree = spec.interactioDegree(level);
        duration = spec.InteractionDuration(level);
        dotDamage = spec.InteractionDamage(level);
    }
}
