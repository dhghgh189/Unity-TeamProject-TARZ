using System.Collections;
using UnityEngine;
using Zenject;

public class LobbySceneInit : MonoBehaviour
{
    [Inject] private StatModel model;
    [Inject] private InGameSaveData saveData;
    [Inject] private PlayerController player;
    [Inject] private Loading loadingObject;

    private void Awake()
    {
        // 데이터 칩 가져오기
        model.Chip = saveData.DataChip;
        // 장비 장착 상황 초기화
        saveData.EquipmentGears = new GearSaveData[(int)Part.Size];
        // 인벤토리 초기화
        saveData.InventoryGears.Clear();
        model.Clear();
        // 블루칩 초기화
        saveData.blueChipSaveDatas.Clear();
        // 인게임 중인지 판단
        if (saveData.chapterSaveData.Chapter > Define.SceneType.Lobby)
        {
            Debug.Log("데이터 있음 이동해야함");
            StartCoroutine(MoveMove());
        }
        else
        {
            Debug.Log("데이터 없음");
        }
    }
    IEnumerator MoveMove()
    {
        while (!loadingObject.IsUnLoading())
        {
            yield return null;
        }
        loadingObject.StartLoading(saveData.chapterSaveData.Chapter);
    }
}
