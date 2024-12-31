using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LobbySceneInit : MonoBehaviour
{
    [Inject] private StatModel model;
    [Inject] private SaveData saveData;

    private void Awake()
    {
        // 장비 장착 상황 초기화
        saveData.EquipmentGears = new GearSaveData[(int)Part.Size];
        // 인벤토리 초기화
        saveData.InventoryGears = new();
        model.Clear();

        // 블루칩 초기화
        // ?
    }
}
