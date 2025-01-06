using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Spec
{
    [HideInInspector]
    public StatModel statModel;
    [Header("Active")]
    [SerializeField] List<float> power;
    [SerializeField] List<float> range;
    [SerializeField] List<float> time;
    [Header("Interaction")]
    [SerializeField, Range(0f, 1f)] List<float> degree;
    [SerializeField] List<float> damage;
    [SerializeField] List<float> duration;

    public float Power(int level) => power[level - 1] * statModel.SkillPowerPer;
    public float Range(int level) => range[level - 1];
    public float Time(int level) => time[level - 1];

    public float interactioDegree(int level) { return (degree.Count > 0) ? degree[level - 1] * statModel.SkillPowerPer : 0; }
    public float InteractionDuration(int level) { return (duration.Count > 0) ? duration[level - 1] : 0; }
    public float InteractionDamage(int level) { return (damage.Count > 0) ? damage[level - 1] : 0; }
}
