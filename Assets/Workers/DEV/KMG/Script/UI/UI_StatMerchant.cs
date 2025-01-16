using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_StatMerchant : MonoBehaviour
{
    [Inject] StatModel statModel;
    [Inject] PlayerController playerController;

    [SerializeField] Button buyButton;
    [SerializeField] Button closeButton;

    [SerializeField] TMP_Text statNameText;
    [SerializeField] TMP_Text statInfoText;
    [SerializeField] TMP_Text priceText;

    private AdditionAbility ability;
    private float value;
    private float price;

    private void Awake()
    {
        BuyStatInit();
        buyButton.onClick.AddListener(BuyStat);
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void BuyStatInit()
    {
        AdditionAbility randomAbility = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size);
        if (randomAbility == AdditionAbility.AllPowerPer)
        {
            randomAbility = Random.Range(0, 2) == 1 ? AdditionAbility.AllPowerPer : (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size);
        }
        ability = randomAbility;
        value = Random.Range(5, 31);
        price = value * 10;
        statNameText.text = $"{ability.ToDescription()} 버프";
        statInfoText.text = $"{ability.ToDescription()}을 {value}만큼 상승시킨다.";
        priceText.text = $"{price} 블랙 데이터 칩";
    }

    private void BuyStat()
    {
        if (statModel.BlackChip < price)
            return;
        statModel.BlackChip -= price;
        statModel.SetAbility(ability, value);
        buyButton.gameObject.SetActive(false);
        priceText.text = "매진";
        SoundManager.PlaySFX(SoundManager.SoundData_UI.Buy);
    }

    private void ClosePanel()
    {
        GetComponentInParent<UI_Merchant>().Temp(UI_Merchant.EMerchant.Stat);
    }
    private void OnEnable()
    {
        buyButton.Select();
    }
}
