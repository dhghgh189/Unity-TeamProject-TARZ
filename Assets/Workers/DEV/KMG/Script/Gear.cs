using System;
using System.Collections.Generic;
using UnityEngine;

public enum Part
{
    모자, 셔츠, 안경, 장갑, 바지, 귀걸이, 반지, 신발, 목걸이, Size
}
[CreateAssetMenu(fileName = "Gear", menuName = "Scriptables/Gear")]
public class Gear : ScriptableObject
{
    public Part Part;
    public int Tier;
    public string GearName;
    public List<GearAbility> Abilities;

    private string[] firstName = { "강력한", "기본적인", "전략적인", "신비로운", "치명적인", "예리한", "건강한", "활발한", "기력의", "흡수하는", "재빠른", "가벼운", "부유한" };
    private string[] lastName = { "전사", "달인", "전술가", "점술가", "CriticalDamage", "칼날", "트레이너", "MaxStaminaPer", "장어", "흡수", "토끼", "주머니", "부자" };
    private string[] tier = { "", "낡은", "일반", "좋은" };
    public void SetName()
    {
        int abilitiesCount = Abilities.Count;

        if (abilitiesCount > 1)
            GearName += $"{firstName[(int)Abilities[1].ability]} ";

        if (abilitiesCount > 2)
            GearName += $"{lastName[(int)Abilities[2].ability]} ";

        GearName += $"{tier[Tier]} {Part}";
    }
}

[Serializable]
public class GearAbility
{
    public AdditionAbility ability;
    public float value;
}
