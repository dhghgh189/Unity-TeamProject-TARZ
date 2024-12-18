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
    public float Angle; // ���� ���� (����)
    public float Range; // ���� ���� (�Ÿ�)
    public float JumpForce; // ���� ���� ���� ��ġ
    public string AnimationName; // ����� �ִϸ��̼� �̸�
    public float Interval; // ��Ʈ�� �ֱ�
    public float ThrowForce;
}
