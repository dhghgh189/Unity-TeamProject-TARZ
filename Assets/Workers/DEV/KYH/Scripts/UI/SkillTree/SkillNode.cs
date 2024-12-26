using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode
{
    public string Name;
    public string Description;
    public AdditionAbility Ability;
    public float Value;
    public float Cost;
    public int[] Condition;

    public SkillNode(string name, string description, AdditionAbility ability, float value, float cost, int[] condition)
    {
        Name = name;
        Description = description;
        Ability = ability;
        Value = value;
        Cost = cost;
        Condition = condition;
    }
}
