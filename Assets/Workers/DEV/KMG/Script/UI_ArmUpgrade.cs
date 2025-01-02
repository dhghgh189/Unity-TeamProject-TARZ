using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UI_ArmUpgrade : MonoBehaviour, ISelectHandler  //, IDeselectHandler
{
    [Inject] SaveData saveData;
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
    private void Start()
    {
        upTier = saveData.ArmUnitInfos[(int)upgrageArmUnit].Tier;
        isInstall = saveData.ArmUnitInfos[(int)upgrageArmUnit].IsInstall;
        if (isInstall)
            UnitInstall();
    }

    private void UnitInstall()
    {
        isInstall = true;
        saveData.ArmUnitInfos[(int)upgrageArmUnit].IsInstall = true;
        armUpgradManager.ArmUnitStatUp(upgradeAbility, upStatList[upTier]);
        SetEventAndDesciption();
    }

    private void UnitUnInstall()
    {
        isInstall = false;
        saveData.ArmUnitInfos[(int)upgrageArmUnit].IsInstall = false;
        armUpgradManager.ArmUnitStatUp(upgradeAbility, -upStatList[upTier]);
        SetEventAndDesciption();
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
        SetEventAndDesciption();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetEventAndDesciption();
    }

    private void SetEventAndDesciption()
    {
        SetUnitbutton.onClick.RemoveAllListeners();
        UnitUpgradebutton.onClick.RemoveAllListeners();

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

        armUpgradManager.SetUpgradeDescription(upgradeName, upgradeDescription, upCostList[upTier].ToString());
    }
}
