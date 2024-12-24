using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_ArmUpgrade : MonoBehaviour
{
    [Inject] SaveData saveData;
    [Inject] ArmUpgradManager armUpgradManager;

    [SerializeField] int upgradeNumber;
    [SerializeField] AdditionAbility upgradeAbility;
    [SerializeField] float upgradeValue;
    [SerializeField] float upgradeCost;

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
            Debug.Log("업글성공");
        }
        else
        {
            Debug.Log("돈없음");
        }
    }
}
