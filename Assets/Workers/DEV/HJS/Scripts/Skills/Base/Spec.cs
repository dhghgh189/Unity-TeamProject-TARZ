using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Spec
{
    [HideInInspector]
    public StatModel statModel;
    [SerializeField] Vector3[] actValue;
    [SerializeField] Vector3[] interactionValue;

    public float Power(int level) { return ( 0 < level && level <= actValue.Length) ? actValue[level - 1].x * statModel.SkillPowerPer : 0; }
    public float Range(int level) { return (0 < level && level <= actValue.Length) ? actValue[level - 1].y : 0; }
    public float Time(int level) { return (0 < level && level <= actValue.Length) ? actValue[level - 1].z : 0; }

    public float InteractionDamage(int level) { return (0 < level && level <= interactionValue.Length) ? interactionValue[level - 1].x * statModel.SkillPowerPer : 0; }
    public float interactioDegree(int level) { return (0 < level && level <= interactionValue.Length) ? interactionValue[level - 1].y * 0.01f : 0; }
    public float InteractionDuration(int level) { return (0 < level && level <= interactionValue.Length) ? interactionValue[level - 1].z : 0; }

    public Vector3[] ActValues { set { actValue = value; } }
    public Vector3[] InteractionValues { set { interactionValue = value; } }
}
