using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum UpgrageArmUnit
{
    DefaultPowerUnit, SkillPowerUnit, ElementalPowerUnit, MaxHpUnit, MaxStaminaUnit, MoveSpeedUnit, Size
}

public class ArmUpgradManager : MonoBehaviour, Base_InteractionOBJ
{
    [Inject] SaveData saveData;
    [Inject] StatModel statModel;
    [Inject] SaveManager saveManager;

    [SerializeField] GameObject upgradePanel;
    [SerializeField] TMP_Text upNameText;
    [SerializeField] TMP_Text upInfoText;
    [SerializeField] TMP_Text upCostText;

    private void Start()
    {

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
            return true;
        }
        return false;
    }

    public void Activate()
    {
        if (upgradePanel.activeSelf)
        {
            upgradePanel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
        Time.timeScale = 0f;
        upgradePanel.SetActive(true);
        GetComponentInChildren<UI_ArmUpgrade>().GetComponent<Button>().Select();
    }
    public void SetUpgradeDescription(string name, string info, string cost)
    {
        upNameText.text = name;
        upInfoText.text = info;
        upCostText.text = cost;
    }
}
