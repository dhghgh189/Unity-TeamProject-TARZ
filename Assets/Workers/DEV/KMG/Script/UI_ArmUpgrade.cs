using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UI_ArmUpgrade : MonoBehaviour, ISelectHandler  //, IDeselectHandler
{
    [Inject] SaveData saveData;
    [Inject] SaveManager saveManager;
    [Inject] ArmUpgradManager armUpgradManager;

    [Header("강화 능력 정보")]
    [SerializeField] UpgrageArmUnit upgrageArmUnit;
    [SerializeField] AdditionAbility upgradeAbility;
    [SerializeField] int upTier;
    [SerializeField] float[] upStatList;
    [SerializeField] float[] upCostList;
    [SerializeField] bool isInstall;
    [Header("강화 능력 설명")]
    [SerializeField] string upgradeName;
    [SerializeField] string upgradeDescription;

    [SerializeField] Button SetUnitbutton;
    [SerializeField] Button UnitUpgradebutton;
    public void SavaDataCheck()
    {
        upTier = saveData.ArmUnitInfos[(int)upgrageArmUnit].Tier;
        isInstall = saveData.ArmUnitInfos[(int)upgrageArmUnit].IsInstall;
        if (isInstall)
            UnitInstall();
    }

    private void UnitInstall()
    {
        if (armUpgradManager.installArmUnits.Count == 3)
            return;
        isInstall = true;
        armUpgradManager.installArmUnits.Add(this);
        saveData.ArmUnitInfos[(int)upgrageArmUnit].IsInstall = true;
        armUpgradManager.ArmUnitStatUp(upgradeAbility, upStatList[upTier]);
        SetEventAndDesciption();
        armUpgradManager.InstallUnitDescription();
        saveManager.Save();
    }

    private void UnitUnInstall()
    {
        isInstall = false;
        armUpgradManager.installArmUnits.Remove(this);
        saveData.ArmUnitInfos[(int)upgrageArmUnit].IsInstall = false;
        armUpgradManager.ArmUnitStatUp(upgradeAbility, -upStatList[upTier]);
        SetEventAndDesciption();
        armUpgradManager.InstallUnitDescription();
    }

    private void UnitUpgrade()
    {
        if (upTier == upStatList.Length - 1 || !armUpgradManager.IsTryUnitUpgrade(upCostList[upTier])) return;

        saveData.ArmUnitInfos[(int)upgrageArmUnit].Tier = ++upTier;
        if (isInstall)
        {
            upTier--;
            UnitUnInstall();
            upTier++;
            UnitInstall();
        }
        armUpgradManager.InstallUnitDescription();
        SetEventAndDesciption();
        saveManager.Save();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetEventAndDesciption();
    }

    private void SetEventAndDesciption()
    {
        SetUnitbutton.onClick.RemoveAllListeners();
        UnitUpgradebutton.onClick.RemoveAllListeners();

        SetUnitbutton.interactable = upTier != 0;

        if (isInstall)
        {
            SetUnitbutton.onClick.AddListener(UnitUnInstall);
            SetUnitbutton.GetComponentInChildren<TMP_Text>().text = "해제";
        }
        else
        {
            SetUnitbutton.onClick.AddListener(UnitInstall);
            SetUnitbutton.GetComponentInChildren<TMP_Text>().text = "장착";
        }

        UnitUpgradebutton.onClick.AddListener(UnitUpgrade);

        armUpgradManager.SetUpgradeDescription(upgradeName, upStatList[upTier] == 0 ? "미획득" : upgradeDescription + $" {upStatList[upTier]}% 강화", upCostList[upTier] == 0 ? "강화완료" : $"{upCostList[upTier].ToString()} 데이터칩");
    }

    public string UnitInfo()
    {
        return $"{upgradeAbility.ToDescription()} {upStatList[upTier]}%\n";
    }
}
