using System.Linq;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    // 인벤토리 슬롯들을 보관할 배열 12개임
    [Inject] UI_GearSlot[] inventorySlots;

    // 장비들의 기본 능력치로 지정된 베이스 장비가 담길 배열
    [SerializeField] Gear[] baseGears = new Gear[(int)Part.Size];

    // 티어와 부위를 지정해 장비를 인벤토리에 저장하는 함수
    public void GetGear(Part part, int tier)
    {
        UI_GearSlot slot = EmptySlot();
        if (!slot) return;

        Gear gear = Instantiate(baseGears.Where(x => x.Part == part).First());

        // 랜덤한 능력치를 상승
        gear.Abilities.Add(new GearAbility() { ability = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size), value = 10 });

        // 이름 변경
        gear.SetName();

        // 베이스 능력치에 티어를 곱하기
        foreach (var item in gear.Abilities)
        {
            item.value *= tier;
        }

        slot.SetGearSlot(gear);
    }
    // 빈 인벤토리 슬롯을 반환하는 함수
    private UI_GearSlot EmptySlot()
    {
        foreach (var item in inventorySlots)
        {
            if (item.IsEmpty) return item;
        }
        return null;
    }

    // 테스트용
    [SerializeField] Part tempPart;
    [SerializeField] int tempTier;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetGear(tempPart, tempTier);
        }
    }
}
