using System;
using System.Collections.Generic;
using UnityEngine;

public enum Part
{
    Hat, Armor, Glasses, Glove, Pants, Earrings, Ring, Shoes, Necklace, Size
}
[CreateAssetMenu(fileName = "Gear", menuName = "Scriptables/Gear")]
public class Gear : ScriptableObject
{
    public Part Part;
    public int Tier;
    public string GearName;
    public List<GearAbility> Abilities;

    public void SetName()
    {
    }
}

[Serializable]
public class GearAbility
{
    public AdditionAbility ability;
    public float value;
}
