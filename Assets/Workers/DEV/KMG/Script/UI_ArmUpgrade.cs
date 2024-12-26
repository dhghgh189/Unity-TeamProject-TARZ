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
        button.onClick.AddListener(ArmUpgredeExecute);
        // 해당 스크립트에 지정된 넘버가 이미 업그레이드 되었다면 버튼을 비활성화
        button.interactable = saveData.ArmUpgradeDatas.Where(x => x.UpgradeNumber == upgradeNumber).Count() == 0;
    }

    private void ArmUpgredeExecute()
    {
        // 돈있음?
        if (armUpgradManager.ArmUpgredeExecute(upgradeNumber, upgradeAbility, upgradeValue, upgradeCost))
        {
            button.interactable = false;
            armUpgradManager.SelecteButton();
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
