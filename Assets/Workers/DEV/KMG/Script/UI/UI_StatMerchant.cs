using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_StatMerchant : MonoBehaviour
{
    [Inject] StatModel statModel;

    private Button buyButton;
    private TMP_Text infoText;

    private AdditionAbility ability;
    float value;
    float price;

    private void Awake()
    {
        buyButton = GetComponentInChildren<Button>();
        infoText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        BuyStatInit();
        buyButton.onClick.AddListener(BuyStat);
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

        infoText.text = $"{price}를 지불하면 {ability.ToDescription()}을 {value}만큼 올려주겠다.";
    }

    private void BuyStat()
    {
        if (statModel.BlackChip < price)
            return;
        statModel.BlackChip -= price;
        statModel.SetAbility(ability, value);
        buyButton.gameObject.SetActive(false);
        infoText.text = "매진이야";
    }

}
