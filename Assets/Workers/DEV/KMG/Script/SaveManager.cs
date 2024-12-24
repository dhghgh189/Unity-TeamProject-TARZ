using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    [Inject] SaveData saveData;
    [Inject] StatModel statModel;
    [Inject] Inventory inventory;
    [Inject] Equipment equipment;

    [ContextMenu("Save")]
    public void Save()
    {
        // statModel을 StatSaveData로 변환시켜서 저장
        saveData.StatSaveData = JsonUtility.FromJson<StatSaveData>(JsonUtility.ToJson(statModel));

        // 장착 장비들 저장
        equipment.EquipmentSave();

        // 인벤토리 저장
        inventory.InventorySave();
        
        // 데이터 칩 저장
        saveData.DataChip = statModel.Chip;

        // PlayerPrefs으로 세이브 데이터 저장
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(saveData));
        Debug.Log(JsonUtility.ToJson(saveData, true));
    }
    [ContextMenu("Reset")]
    public void Reset()
    {
        PlayerPrefs.DeleteKey("SaveData");
    }
}
