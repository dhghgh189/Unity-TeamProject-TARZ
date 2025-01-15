using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_EquipmentMerchant : MonoBehaviour
{
    [Inject] Inventory inventory;
    [Inject] StatModel statModel;
    [Inject] PlayerController playerController;

    private Gear gear;
    private float price;

    [SerializeField] Button buyButton;
    [SerializeField] Button closeButton;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text chipText;

    [SerializeField] Image itemImage;

    private bool oneMoreTime;

    private void Awake()
    {
        buyButton.onClick.AddListener(BuyGear);
        closeButton.onClick.AddListener(ClosePanel);
        SetSellGear();
    }
    private void SetSellGear()
    {
        gear = inventory.StoreGear();
        nameText.text = gear.GearName;
        infoText.text = string.Empty;
        foreach (var item in gear.Abilities)
        {
            infoText.text += $"{item.ability.ToDescription()} {item.value}\n";
        }
        price = gear.Tier * 100;
        chipText.text = $"{price} 블랙 데이터 칩";
        itemImage.sprite = inventory.GetSprite(((int)gear.Part * 3) + (gear.Tier - 1));
    }

    private void BuyGear()
    {
        if (statModel.BlackChip < price || !inventory.GetGear(gear))
            return;
        statModel.BlackChip -= price;

        SoundManager.PlaySFX(SoundManager.SoundData_UI.Buy);

        if (!oneMoreTime && Util.IsRandom(50))
        {
            SetSellGear();
            oneMoreTime = true;
            return;
        }

        buyButton.gameObject.SetActive(false);
        infoText.text = "매진";
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
