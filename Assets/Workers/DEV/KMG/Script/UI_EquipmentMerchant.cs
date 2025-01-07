using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_EquipmentMerchant : MonoBehaviour
{
    [Inject] Inventory inventory;
    [Inject] StatModel statModel;

    private Gear gear;
    private float price;

    private Button buyButton;
    private TMP_Text infoText;

    private bool oneMoreTime;

    private void Awake()
    {
        buyButton = GetComponentInChildren<Button>();
        infoText = GetComponentInChildren<TMP_Text>();

        buyButton.onClick.AddListener(BuyGear);
    }
    private void Start()
    {
        SetSellGear();
    }
    private void SetSellGear()
    {
        gear = inventory.StoreGear();
        infoText.text = string.Empty;
        foreach (var item in gear.Abilities)
        {
            infoText.text += $"{item.ability.ToDescription()} {item.value}\n";
        }
        price = gear.Tier * 100;
        infoText.text += $"{gear.GearName}\n 이 장비를 {price}원에 사쉴?";
    }

    private void BuyGear()
    {
        if (statModel.BlackChip < price || !inventory.GetGear(gear))
            return;
        statModel.BlackChip -= price;

        if (!oneMoreTime && Util.IsRandom(50))
        {
            SetSellGear();
            oneMoreTime = true;
            return;
        }

        buyButton.gameObject.SetActive(false);
        infoText.text = "매진이야";
    }
}
