using System;
using System.Collections.Generic;

public class SaveData
{
    // 패시브 강화를 위한 비휘발성 재화
    public float DataChip;
    // 스탯을 저장하는 클래스
    public StatSaveData StatSaveData;
    // 장착된 장비를 저장하는 배열
    public GearSaveData[] EquipmentGears = new GearSaveData[(int)Part.Size];
    // 인벤토리의 장비들을 저장하는 리스트
    public List<GearSaveData> InventoryGears = new();
    // 패시브 강화 상태를 저장하는 리스트
    public List<ArmUpgrade> ArmUpgradeDatas = new();
}
// MonoBehaviour를 상속한 클래스들은 FromJson으로 역 직렬화가 불가능 함
// 그러므로 아래와 같은 클래스들을 만듬

[Serializable]
public class StatSaveData
{
    public float maxHp;
    public float maxStamina;
    public float currentHp;
    public float currentMp;
    public float currentStamina;
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
[Serializable]
public class ArmUpgrade
{
    public int UpgradeNumber;
    public AdditionAbility UpgradeAbility;
    public float UpgradeValue;
    public ArmUpgrade(int upgradeNumber, AdditionAbility upgradeAbility, float upgradeValue)
    {
        UpgradeNumber = upgradeNumber;
        UpgradeAbility = upgradeAbility;
        UpgradeValue = upgradeValue;
    }
}
