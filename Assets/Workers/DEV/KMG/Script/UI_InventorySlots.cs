using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UI_InventorySlots : MonoBehaviour
{
    [Inject] StatModel statModel;
    [Inject] Equipment equipment;
    [Inject] Inventory inventory;
    [SerializeField] Gear gear;
    [SerializeField] TMP_Text gearName;
    public bool IsEmpty = true;
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
        gear = null;
        IsEmpty = true;
        gearName.text = string.Empty;

        // 이전 장비 부위가 있다면 슬롯에 저장
        if (oldGear)
            SetInventorySlots(oldGear);
    }
    // 해당 슬롯에 장비를 보관시키는 함수
    public void SetInventorySlots(Gear gear)
    {
        if(gearName == null)
            gearName = GetComponentInChildren<TMP_Text>();

        IsEmpty = false;
        this.gear = gear;
        gearName.text = gear.GearName;
    }

    public void GearSell()
    {
        statModel.Chip += gear.Tier;
        gear = null;
        IsEmpty = true;
        gearName.text = string.Empty;
    }

    public GearSaveData SaveInventoyGear()
    {
        if (!gear)
            return null;
        return JsonUtility.FromJson<GearSaveData>(JsonUtility.ToJson(gear));
    }
}
