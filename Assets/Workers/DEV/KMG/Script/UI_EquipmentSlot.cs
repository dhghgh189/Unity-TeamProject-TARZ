using TMPro;
using UnityEngine;

public class UI_EquipmentSlot : MonoBehaviour
{
    [SerializeField] Gear gear;
    private TMP_Text gearNameText;
    private void Awake()
    {
        gearNameText = GetComponentInChildren<TMP_Text>();
    }
    public void SetEquipmentSlot(Gear gear)
    {
        this.gear = gear;
        gearNameText.text = gear.GearName;
    }
}
