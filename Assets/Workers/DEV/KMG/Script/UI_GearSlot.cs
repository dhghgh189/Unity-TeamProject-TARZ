using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UI_GearSlot : MonoBehaviour, IPointerClickHandler
{
    [Inject] Equipment equipment;
    [SerializeField] Gear gear;
    [SerializeField] TMP_Text gearName;
    public bool IsEmpty = true;
    private void Start()
    {
        gearName = GetComponentInChildren<TMP_Text>();
    }
    // 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gear) return;
        equipment.ChangeGear(gear);
        gear = null;
        IsEmpty = true;
        gearName.text = string.Empty;
    }
    // 해당 슬롯에 장비를 보관시키는 함수
    public void SetGearSlot(Gear gear)
    {
        IsEmpty = false;
        this.gear = gear;
        gearName.text = gear.GearName;
    }
}
