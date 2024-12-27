using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UI_ArmUpgrade : MonoBehaviour, ISelectHandler
{
    [Inject] SaveData saveData; 
    [Inject] ArmUpgradManager armUpgradManager;

    [Header("강화 능력 정보")]
    [SerializeField] int upgradeNumber;
    [SerializeField] AdditionAbility upgradeAbility;
    [SerializeField] float upgradeValue;
    [SerializeField] float upgradeCost;
    [Header("강화 능력 설명")]
    [SerializeField] string upgradeName;
    [SerializeField] string upgradeDescription;

    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        // 강화가 안되어 있으면 이벤트를 추가
        if (saveData.ArmUpgradeDatas.Where(x => x.UpgradeNumber == upgradeNumber).Count() == 0)
        {
            button.onClick.AddListener(ArmUpgredeExecute);
        }
    }

    private void ArmUpgredeExecute()
    {
        // 돈있음?
        if (armUpgradManager.ArmUpgredeExecute(upgradeNumber, upgradeAbility, upgradeValue, upgradeCost))
        {
            button.onClick.RemoveAllListeners();
        }
        else
        {
            Debug.Log("돈없음");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        armUpgradManager.SetUpgradeDescription(upgradeName, upgradeDescription, upgradeCost.ToString());
    }
}
