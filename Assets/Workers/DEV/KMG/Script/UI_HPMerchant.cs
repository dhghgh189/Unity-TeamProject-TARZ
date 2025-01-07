using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_HPMerchant : MonoBehaviour
{
    [Inject] StatModel statModel;
    
    [SerializeField] float price;

    private Button buyButton;
    private TMP_Text infoText;

    private void Start()
    {
        buyButton = GetComponentInChildren<Button>();
        infoText = GetComponentInChildren<TMP_Text>();
        buyButton.onClick.AddListener(HPBuy);
    }

    private void HPBuy()
    {
        if (statModel.BlackChip < price)
            return;
        statModel.BlackChip -= price;
        statModel.CurrentHp += statModel.MaxHp * 0.33f;
        buyButton.gameObject.SetActive(false);
        infoText.text = "매진이야";
    }
}
