using System;
using System.Collections.Generic;

public class SaveData
{
    public StatSaveData StatSaveData;
    public GearSaveData[] EquipmentGears = new GearSaveData[(int)Part.Size];
    public List<GearSaveData> InventoryGears = new();
}
// MonoBehaviour를 상속한 클래스들은 FromJson으로 역 직렬화가 불가능 함
// 그러므로 아래와 같은 클래스들을 만듬

[Serializable]
public class StatSaveData
{
    public float maxHp;
    public float maxStamina;
    public float allPower;
    public float currentHp;
    public float currentMp;
    public float currentStamina;
    public float chip;
    public float blackChip;
    public float[] additionAbility = new float[(int)AdditionAbility.Size];
}
[Serializable]
public class GearSaveData
{
    public Part Part;
    public int Tier = 0;
    public string GearName;
    public List<GearAbility> Abilities;
}
