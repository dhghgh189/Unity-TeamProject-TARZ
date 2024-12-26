using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

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
        // 세이브 데이터의 업글 정보를 순회 하면서 모델에 반영
        foreach (ArmUpgrade item in saveData.ArmUpgradeDatas)
        {
            statModel.SetAbility(item.UpgradeAbility, item.UpgradeValue);
        }
    }
    // UI_ArmUpgrade의 버튼이 클릭되면 스탯을 반영하고 바로 저장함
    public bool ArmUpgredeExecute(int upNumber, AdditionAbility ability, float value, float cost)
    {
        if (statModel.Chip < cost) return false;

        statModel.Chip -= cost;
        statModel.SetAbility(ability, value);
        saveData.ArmUpgradeDatas.Add(new ArmUpgrade(upNumber, ability, value));
        saveManager.Save();

        return true;
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
        SelecteButton();
    }
    public void SetUpgradeDescription(string name, string info, string cost)
    {
        upNameText.text = name;
        upInfoText.text = info;
        upCostText.text = cost;
    }

    public void SelecteButton()
    {
        foreach (var item in GetComponentsInChildren<Button>())
        {
            if (item.interactable)
            {
                item.Select();
                return;
            }
        }
    }
}
