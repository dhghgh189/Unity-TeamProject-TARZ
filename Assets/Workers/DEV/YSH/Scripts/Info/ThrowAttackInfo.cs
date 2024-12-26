using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMultiActionType { Basic, Horizontal, Vertical, Length }

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
    public int StackAmount;     // 필요한 물건스택의 수
    public float Damage;
    public float ThrowForce;
    public EffectInfo EffectInfo;
}
