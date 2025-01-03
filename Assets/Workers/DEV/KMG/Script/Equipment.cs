using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Equipment : MonoBehaviour
{
    [Inject] StatModel statModel;
    // 장착된 장비를 보관하는 배열
    [SerializeField] Gear[] equipmentGears = new Gear[(int)Part.Size];

    [Inject] UI_EquipmentSlot[] uI_EquipmentSlots = new UI_EquipmentSlot[(int)Part.Size];

    [Inject] SaveData saveData;
    private void Start()
    {
        foreach (GearSaveData item in saveData.EquipmentGears)
        {
            if (item == null || item.Tier == 0) continue;
            Gear saveGear = ScriptableObject.CreateInstance<Gear>();
            saveGear.Part = item.Part;
            saveGear.Tier = item.Tier;
            saveGear.GearName = item.GearName;
            saveGear.Abilities = item.Abilities;
            equipmentGears[(int)item.Part] = saveGear;
            uI_EquipmentSlots[(int)item.Part].SetEquipmentSlot(saveGear);
        }
    }

    public void ChangeGear(Gear gear, out Gear oldGear)
    {
        oldGear = null;
        // 해당 파츠를 이미 장착한 상태라면 해당 장비의 능력치만큼 스탯을 감소
        if (equipmentGears[(int)gear.Part])
        {
            foreach (GearAbility gearAbility in equipmentGears[(int)gear.Part].Abilities)
            {
                statModel.SetAbility(gearAbility.ability, -gearAbility.value);
            }
            oldGear = equipmentGears[(int)gear.Part];
        }
        // 장비의 능력치 만큼 스탯을 증가
        foreach (GearAbility gearAbility in gear.Abilities)
        {
            statModel.SetAbility(gearAbility.ability, gearAbility.value);
            equipmentGears[(int)gear.Part] = gear;
            uI_EquipmentSlots[(int)gear.Part].SetEquipmentSlot(gear);
        }
    }
    public void EquipmentSave()
    {
        for (int i = 0; i < (int)Part.Size; i++)
        {
            if (!equipmentGears[i]) continue;
            saveData.EquipmentGears[i] = JsonUtility.FromJson<GearSaveData>(JsonUtility.ToJson(equipmentGears[i]));
        }
    }
}
