using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public enum UpgrageArmUnit
{
    DefaultPowerUnit, SkillPowerUnit, ElementalPowerUnit, MaxHpUnit, MaxStaminaUnit, MoveSpeedUnit, Size
}

public class ArmUpgradManager : MonoBehaviour, Interaction_Ibase_Activate
{
    [Inject] StatModel statModel;
    [Inject] SaveManager saveManager;
    [Inject] PlayerController playerController;

    private UI_ArmUpgrade[] armUnits;
    public List<UI_ArmUpgrade> installArmUnits;

    [SerializeField] GameObject upgradePanel;
    [SerializeField] TMP_Text upNameText;
    [SerializeField] TMP_Text upInfoText;
    [SerializeField] TMP_Text upCostText;
    [SerializeField] TMP_Text installUnitsText;

    [SerializeField] TMP_Text dataChipText;

    private void Start()
    {
        armUnits = GetComponentsInChildren<UI_ArmUpgrade>(true);
        foreach (var item in armUnits)
        {
            item.SavaDataCheck();
        }

        statModel.OnChipChange += DataChipChange;
        dataChipText.text = $"{statModel.Chip}";
    }
    public void ArmUnitStatUp(AdditionAbility ability, float value)
    {
        statModel.SetAbility(ability, value);
    }
    public bool IsTryUnitUpgrade(float chip)
    {
        if (statModel.Chip >= chip)
        {
            statModel.Chip -= chip;
            SoundManager.PlaySFX(SoundManager.SoundData_UI.UpSucces);
            return true;
        }
        SoundManager.PlaySFX(SoundManager.SoundData_UI.UpFaild);
        return false;
    }

    public void Activate()
    {
        if (upgradePanel.activeSelf)
        {
            upgradePanel.SetActive(false);
            OnlyInteractAction(false);
            saveManager.Save();
            return;
        }
        OnlyInteractAction(true);
        upgradePanel.SetActive(true);
        GetComponentInChildren<UI_ArmUpgrade>().GetComponent<Button>().Select();
    }
    public void SetUpgradeDescription(string name, string info, string cost)
    {
        upNameText.text = name;
        upInfoText.text = info;
        upCostText.text = cost;
    }
    public void InstallUnitDescription()
    {
        installUnitsText.text = "";
        foreach (var item in installArmUnits)
        {
            installUnitsText.text += item.UnitInfo();
        }
    }

    private void DataChipChange(float chip)
    {
        dataChipText.text = $"{chip}";
    }
    private void OnlyInteractAction(bool on)
    {
        InputActionMap actionMap = InputSystem.actions.FindActionMap("Player");
        if (on)
        {
            foreach (var action in actionMap.actions)
            {
                if (action.name != "Interact")
                {
                    action.Disable();
                }
            }
        }
        else
        {
            actionMap.Enable();
        }
    }
}
