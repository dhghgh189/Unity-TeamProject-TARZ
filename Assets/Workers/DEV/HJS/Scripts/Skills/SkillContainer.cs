using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Skill Container")]
public class SkillContainer : ScriptableObject
{
    public List<BaseSkillSO> Skills;
}
