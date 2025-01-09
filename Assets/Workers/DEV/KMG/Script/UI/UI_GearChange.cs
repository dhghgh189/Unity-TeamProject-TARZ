using TMPro;
using UnityEngine;
using Zenject;

public class UI_GearChange : MonoBehaviour
{
    [Inject] Equipment equipment;

    [SerializeField] GameObject wearedGearPanel;
    [SerializeField] GameObject inventoryGearPanel;
    [SerializeField] GameObject arrowImg;

    [SerializeField] TMP_Text wearedGearName;
    [SerializeField] TMP_Text wearedGearPart;
    [SerializeField] TMP_Text wearedGearTier;
    [SerializeField] TMP_Text wearedGearStat;

    [SerializeField] TMP_Text inventoryGearName;
    [SerializeField] TMP_Text inventoryGearPart;
    [SerializeField] TMP_Text inventoryGearTier;
    [SerializeField] TMP_Text inventoryGearStat;

    public void SetGearInfo(Gear gear)
    {
        if (!gear)
        {
            inventoryGearPanel.SetActive(false);
            wearedGearPanel.SetActive(false);
            arrowImg.SetActive(false);
            return;
        }
        InventoryGearInfoSet(gear);
        Gear wearedPartGear = equipment.GetPartGear(gear.Part);
        if (wearedPartGear != null)
        {
            WearedGearInfoSet(wearedPartGear);
        }
    }

    private void WearedGearInfoSet(Gear gear)
    {
        wearedGearPanel.SetActive(true);
        arrowImg.SetActive(true);

        wearedGearName.text = gear.GearName;
        wearedGearPart.text = $"{gear.Part}";
        wearedGearTier.text = $"{gear.Tier}";

        wearedGearStat.text = string.Empty ;
        foreach (var item in gear.Abilities)
        {
            wearedGearStat.text += $"{item.ability.ToDescription()} + {item.value}\n";
        }
    }

    private void InventoryGearInfoSet(Gear gear)
    {
        inventoryGearPanel.SetActive(true);
        wearedGearPanel.SetActive(false);
        arrowImg.SetActive(false);

        inventoryGearName.text = gear.GearName;
        inventoryGearPart.text = $"{gear.Part}";
        inventoryGearTier.text = $"{gear.Tier}";

        inventoryGearStat.text = string.Empty ;
        foreach (var item in gear.Abilities)
        {
            inventoryGearStat.text += $"{item.ability.ToDescription()} + {item.value}\n";
        }
    }
}
