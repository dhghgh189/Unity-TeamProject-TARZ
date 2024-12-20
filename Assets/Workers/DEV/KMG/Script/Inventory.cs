using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [Inject] SaveData saveData;

    // 인벤토리 슬롯들을 보관할 배열 12개임
    [Inject] UI_InventorySlots[] inventorySlots;

    // 장비들의 기본 능력치로 지정된 베이스 장비가 담길 배열
    [SerializeField] Gear[] baseGears = new Gear[(int)Part.Size];

    private void Start()
    {
        foreach (var item in saveData.InventoryGears)
        {
            Gear saveGear = ScriptableObject.CreateInstance<Gear>();
            saveGear.Part = item.Part;
            saveGear.Tier = item.Tier;
            saveGear.GearName = item.GearName;
            saveGear.Abilities = item.Abilities;
            EmptySlot().SetInventorySlots(saveGear);
        }
    }

    // 티어와 부위를 지정해 장비를 인벤토리에 저장하는 함수
    public void GetGear(Part part, int tier)
    {
        UI_InventorySlots slot = EmptySlot();
        if (!slot) return;

        // 해당 부위의 베이스 장비를 가져옴
        Gear gear = Instantiate(baseGears.Where(x => x.Part == part).First());

        // 티어 부여
        tier = Mathf.Clamp(tier, 1, 3);
        gear.Tier = tier;

        // 장갑은 4개중 하나의 기본 능력치를 가지므로 능력치 3개를 삭제
        if (part == Part.장갑)
        {
            for (int i = 0; i < 3; i++)
            {
                gear.Abilities.RemoveAt(Random.Range(0, gear.Abilities.Count));
            }
        }

        // 랜덤한 능력치를 랜덤 확률로 상승
        if (Util.IsRandom(50))
            gear.Abilities.Add(new GearAbility() { ability = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size), value = 10 });
        if (Util.IsRandom(50))
            gear.Abilities.Add(new GearAbility() { ability = (AdditionAbility)Random.Range(0, (int)AdditionAbility.Size), value = 10 });

        // 이름 변경
        gear.SetName();

        // 베이스 능력치에 티어를 곱하기
        foreach (var item in gear.Abilities)
        {
            item.value *= tier;
        }

        slot.SetInventorySlots(gear);
    }
    // 빈 인벤토리 슬롯을 반환하는 함수
    private UI_InventorySlots EmptySlot()
    {
        foreach (var item in inventorySlots)
        {
            if (item.IsEmpty) return item;
        }
        return null;
    }

    public void InventorySave()
    {
        saveData.InventoryGears.Clear();
        foreach (var item in inventorySlots)
        {
            GearSaveData gearSaveData = item.SaveInventoyGear();
            if (gearSaveData == null) continue;
            saveData.InventoryGears.Add(gearSaveData);
        }
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetGear((Part)Random.Range(0, (int)Part.Size), Random.Range(1, 4));
        }
    }
}
