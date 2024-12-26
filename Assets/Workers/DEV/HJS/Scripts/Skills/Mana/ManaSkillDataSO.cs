using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[Flags]
public enum ManaSkillSpec 
{ 
    none = 0, 
    Damage = 1 << 0, 
    AttackRange = 1 << 1, 
    TargetSpeed = 1 << 2,
    TargetTime = 1 << 3,
    RushSpeed = 1 << 4,
    RushTime = 1 << 5,
    UniqueFeatureRange = 1 << 6,
    UniqueFeatureSpeed = 1 << 7
}
public enum ManaSkillType { Skill_1, Skill_2, Skill_3, Skill_4 }
[CreateAssetMenu(menuName = "Scriptables/ManaSkillDataSO")]
public class ManaSkillDataSO : ScriptableObject
{
    [Header("Info")]
    public ManaSkillSpec spec;
    public ManaSkillType Type;
    public string skillName;

    [Header("Setting")]
    [SerializeField, Tooltip("스킬의 데미지")] float damage;
    [SerializeField, Tooltip("스킬의 공격 범위")] float attackRange;
    [SerializeField, Tooltip("스킬의 투사체 속도")] float targetSpeed;
    [SerializeField, Tooltip("스킬의 투사체 시간")] float targetTime;
    [SerializeField, Tooltip("스킬의 돌진속도")] float rushSpeed;
    [SerializeField, Tooltip("스킬의 돌진시간")] float rushTime;
    [SerializeField, Tooltip("스킬의 고유 기능의 범위(ex.흡수 범위)")] float uniqueFeatureRange;
    [SerializeField, Tooltip("스킬의 고유 기능의 시간(ex.흡수 시간)")] float uniqueFeatureSpeed;

    #region 프로퍼티
    public float Damage => damage;
    public float AttackRange => attackRange;
    public float TargetSpeed => targetSpeed;
    public float TargetTime => targetTime;
    public float RushSpeed => rushSpeed;
    public float RushTime => rushTime;
    public float UniqueFeatureRange => uniqueFeatureRange;
    public float UniqueFeatureSpeed => uniqueFeatureSpeed;
    #endregion
}
