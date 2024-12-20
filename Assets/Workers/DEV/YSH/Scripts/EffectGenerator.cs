using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffectType { KnockBack }

public class EffectGenerator
{
    public IEffect Create(EEffectType type)
    {
        IEffect effect = null;
        switch (type)
        {
            case EEffectType.KnockBack:
                effect = new KnockBack();
                break;
        }

        return effect;
    }
}
