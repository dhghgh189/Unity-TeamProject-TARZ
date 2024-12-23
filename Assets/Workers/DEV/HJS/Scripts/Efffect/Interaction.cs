using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SkillEnum;

public class Interaction : IEffect, ISpec
{
    public InteractionType type { get; set; }

    private float degree;
    private float dotDamage;
    private float duration;

    public Interaction(InteractionType type)
    {
        this.type = type;
        degree = 0f;
        dotDamage = 0f;
        duration = 0f;
    }

    public void Activate(GameObject attacker, GameObject target)
    {
        Debug.Log("<color=red>Activate Abnormal status</color>");
        IStatusEffect statusEffectable = target.GetComponent<IStatusEffect>();

        switch(type)
        {
            case InteractionType.Slow:
                statusEffectable?.SlowEffect(attacker, target, (1f - degree), duration);
                break;
            case InteractionType.Elec:
                statusEffectable?.ElectroEffect(attacker, target, dotDamage, duration);
                break;
            case InteractionType.Frozen:
                statusEffectable?.FrozenEffect(attacker, target, duration);
                break;
            case InteractionType.Damage:
                IDamagable damagable = target.GetComponent<IDamagable>();
                damagable?.TakeDamage((int)dotDamage);
                break;
            default: 
                break;
        }
    }

    public void SetSpec(SkillSpecDatabase.Spec spec, int level)
    {
        degree = spec.interactioDegree(level);
        duration = spec.InteractionDuration(level);
        dotDamage = spec.InteractionDamage(level);
    }
}
