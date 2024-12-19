using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/MonsterSkill")]
public class MonsterSkill : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public int Damage;
    public float CoolTime;
    public float Angle; // 공격 범위 (각도)
    public float Range; // 공격 범위 (거리)
    public float JumpForce; // 점프 공격 점프 수치
    public string AnimationName; // 출력할 애니메이션 이름
    public float Interval; // 도트뎀 주기
    public float ThrowForce;
    public bool CanUseSkill = true; // 기본값 true
}
