using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UI_InventorySlots : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Inject] StatModel statModel;
    [Inject] Equipment equipment;
    [Inject] Inventory inventory;

    public bool IsEmpty = true;
    [SerializeField] Gear gear;

    private Image image;
    private GameObject outline;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => inventory.SelectSlot(this));
    }
    public void EquipGear()
    {
        // 장착할 장비 부위를 이미 장착하면 oldGear에 저장
        Gear oldGear = null;
        equipment.ChangeGear(gear, out oldGear);

        // 장착하고 인벤토리 슬롯을 초기화
        ClearSlot();

        // 이전 장비 부위가 있다면 슬롯에 저장
        if (oldGear)
            SetInventorySlots(oldGear);
    }
    // 해당 슬롯에 장비를 보관시키는 함수
    public void SetInventorySlots(Gear gear)
    {
        IsEmpty = false;
        this.gear = gear;

        if (!image)
            image = GetComponent<Image>();

        image.sprite = inventory.GetSprite(((int)gear.Part * 3) + (gear.Tier - 1));
    }

    public void GearSell()
    {
        statModel.BlackChip += gear.Tier + 1;
        SoundManager.PlaySFX(SoundManager.SoundData_UI.DecompositEquip);
        ClearSlot();
    }

    public GearSaveData SaveInventoyGear()
    {
        if (!gear)
            return null;
        return JsonUtility.FromJson<GearSaveData>(JsonUtility.ToJson(gear));
    }

    public void ClearSlot()
    {
        // 장착하고 인벤토리 슬롯을 초기화
        gear = null;
        IsEmpty = true;
        image.sprite = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!outline) 
            outline = GetComponentsInChildren<Image>(true)[1].gameObject;
        outline.SetActive(true);
        inventory.GearChange.SetGearInfo(gear);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        outline.SetActive(false);
    }
}
