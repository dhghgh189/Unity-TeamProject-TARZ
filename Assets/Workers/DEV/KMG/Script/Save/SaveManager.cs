using System.IO;
using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    [Inject] SaveSlotData slotData;
    [Inject] StatModel statModel;
    [Inject] Inventory inventory;
    [Inject] Equipment equipment;
    [Inject] PlayerSkillHandler handler;

    [ContextMenu("Save")]
    public void Save()
    {
        // statModel을 StatSaveData로 변환시켜서 저장
        slotData.InGameSaveData.StatSaveData = JsonUtility.FromJson<StatSaveData>(JsonUtility.ToJson(statModel));

        // 장착 장비들 저장
        equipment.EquipmentSave();

        // 인벤토리 저장
        inventory.InventorySave();

        // 데이터 칩 저장
        slotData.DataChip = statModel.Chip;

        slotData.InGameSaveData.blueChipSaveDatas = handler.SaveBlueChips();

        // PlayerPrefs으로 세이브 데이터 저장
        if (string.IsNullOrEmpty(slotData.SlotPath))
            return;   
        File.WriteAllText(slotData.SlotPath, JsonUtility.ToJson(slotData));
        Debug.Log(File.ReadAllText(slotData.SlotPath));
    }
    [ContextMenu("Reset")]
    public void Reset()
    {
        PlayerPrefs.DeleteKey("SaveData");
    }
}
