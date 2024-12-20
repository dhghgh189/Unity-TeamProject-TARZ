using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UI_InventorySlots : MonoBehaviour, IPointerClickHandler
{
    [Inject] StatModel statModel;
    [Inject] Equipment equipment;
    [SerializeField] Gear gear;
    [SerializeField] TMP_Text gearName;
    public bool IsEmpty = true;
    private void Start()
    {
        gearName = GetComponentInChildren<TMP_Text>();
    }
    // 클릭 이벤트, 원작 게임은 마우스 클릭이 없음 변경해야할듯
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            GearSell();
            return;
        }
        if (!gear) return;
        EquipGear();
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
        IsEmpty = false;
        this.gear = gear;
        gearName.text = gear.GearName;
    }

    private void GearSell()
    {
        statModel.Chip += gear.Tier;
        gear = null;
        IsEmpty = true;
        gearName.text = string.Empty;
    }
}
