using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StatTest : MonoBehaviour
{
    [Inject] StatModel statModel;
    [Inject] SaveData saveData;
    [Inject] Inventory inventory;
    [Inject] Equipment equipment;
    public void PlayerHit(float value)
    {
        statModel.CurrentHp -= value;
    }
    public void DefaultAtack()
    {
        Debug.Log(statModel.DefaultPower);
    }
    public void SkillAtack()
    {
        Debug.Log(statModel.SkillPower);
    }
    public void ElementalAtack()
    {
        Debug.Log(statModel.ElementalPower);
    }
    [ContextMenu("Save")]
    public void Save()
    {
        // statModel을 StatSaveData로 변환시켜서 저장
        saveData.StatSaveData = JsonUtility.FromJson<StatSaveData>(JsonUtility.ToJson(statModel));

        // 장착 장비들 저장
        equipment.EquipmentSave();

        // 인벤토리 저장
        inventory.InventorySave();

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
