using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectInfo
{
    public EffectData[] EffectDatas;
}

[System.Serializable]
public class EffectData
{
    public EEffectType EffectType;
    public IEffect Effect { get; set; }
}
