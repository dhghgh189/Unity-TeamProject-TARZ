using System;
using System.Linq;

public class SaveSlotData
{
    public long CreateTime;

    public string SlotPath;

    public float DataChip;

    // 암 유닛 강화 상태를 저장하는 리스트
    public ArmUnitInfo[] ArmUnitInfos = Enumerable.Range(0, (int)UpgrageArmUnit.Size).Select(_ => new ArmUnitInfo()).ToArray();

    public InGameSaveData InGameSaveData = new();
}

[Serializable]
public class ArmUnitInfo
{
    public int Tier;
    public bool IsInstall;
}
