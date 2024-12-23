using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMultiActionType { Basic, Left, Right, Length }

[System.Serializable]
public class ThrowAttackInfo
{
    public MultiActionInfo[] MultiActions;
    public float Damage;
    public float ThrowForce;
    public EffectInfo EffectInfo;
}

[System.Serializable]
public class MultiActionInfo
{
    public EMultiActionType ActionType;
    public float Damage;
    public float ThrowForce;
    public EffectInfo EffectInfo;
}
