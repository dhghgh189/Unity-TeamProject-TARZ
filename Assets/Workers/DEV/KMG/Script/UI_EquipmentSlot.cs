using TMPro;
using UnityEngine;

public class UI_EquipmentSlot : MonoBehaviour
{
    [SerializeField] Gear gear;
    private TMP_Text gearNameText;
    public void SetEquipmentSlot(Gear gear)
    {
        if (gearNameText == null)
            gearNameText = GetComponentInChildren<TMP_Text>();
        this.gear = gear;
        gearNameText.text = gear.GearName;
    }
}
