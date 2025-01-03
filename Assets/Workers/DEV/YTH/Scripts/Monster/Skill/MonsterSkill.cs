using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/MonsterSkill")]
public class MonsterSkill : ScriptableObject
{
    public string SkillName { get; set; }
    public string SkillDescription { get; set; }
    public float Damage { get; set; }
    public float CoolTime { get; set; }
    public float Angle { get; set; } // 공격 범위 (각도)
    public float Range { get; set; } // 공격 범위 (거리)
    
    public float Interval { get; set; } // 도트뎀 주기
    public float ThrowForce { get; set; }
    public bool CanUseSkill { get; set; }// 기본값 true
    public float Duration { get; set; }

    // 점프어택, 대쉬어택
    public float InAirTime { get; set; } // 체공 시간
    public float JumpHeight { get; set; } // Y축 점프 높이
    public float JumpDistance { get; set; } // Z축 점프 거리
}
