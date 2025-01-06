using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_EquipmentSlot : MonoBehaviour
{
    [Inject] Inventory inventory;

    [SerializeField] Gear gear;

    private Image image;

    public void SetEquipmentSlot(Gear gear)
    {
        this.gear = gear;
        if (!image)
            image = GetComponent<Image>();
        image.sprite = inventory.GetSprite(((int)gear.Part * 3) + (gear.Tier - 1));
    }
}
