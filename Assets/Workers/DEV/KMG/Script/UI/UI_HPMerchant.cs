using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_HPMerchant : MonoBehaviour
{
    [Inject] PlayerController playerController;
    [Inject] StatModel statModel;
    
    [SerializeField] float price;

    [SerializeField] Button buyButton;
    [SerializeField] Button closeButton;
    [SerializeField] TMP_Text ChipText;

    private void Start()
    {
        buyButton = GetComponentInChildren<Button>();
        buyButton.onClick.AddListener(HPBuy);
        closeButton.onClick.AddListener(ClosePanel);

        ChipText.text = $"{price} 블랙 데이터 칩";
    }

    private void HPBuy()
    {
        if (statModel.BlackChip < price)
            return;
        statModel.BlackChip -= price;
        statModel.CurrentHp += statModel.MaxHp * 0.33f;
        buyButton.gameObject.SetActive(false);
        ChipText.text = "매진";
    }

    private void ClosePanel()
    {
        playerController.PInput.IsCanControl = true;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        buyButton.Select();
    }
}
