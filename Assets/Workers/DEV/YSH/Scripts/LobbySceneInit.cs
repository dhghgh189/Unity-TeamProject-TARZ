using System.Collections;
using UnityEngine;
using Zenject;

public class LobbySceneInit : MonoBehaviour
{
    [Inject] private StatModel model;
    [Inject] private SaveSlotData saveData;
    [Inject] private Loading loadingObject;
    [Inject] private ThrowObjectStack throwObjectStack;

    private void Awake()
    {
        // 데이터 칩 가져오기
        model.Chip = saveData.DataChip;

        // 인게임 중인지 판단
        if (saveData.InGameSaveData.chapterSaveData.Chapter > Define.SceneType.Lobby)
        {
            Debug.Log("데이터 있음 이동해야함");
            StartCoroutine(MoveMove());
            return;
        }
        else
        {
            Debug.Log("데이터 없음");
        }

        // 장비 장착 상황 초기화
        saveData.InGameSaveData.EquipmentGears = new GearSaveData[(int)Part.Size];
        // 인벤토리 초기화
        saveData.InGameSaveData.InventoryGears.Clear();
        model.Clear();
        // 블루칩 초기화
        saveData.InGameSaveData.blueChipSaveDatas.Clear();
        // 쓰레기 초기화
        throwObjectStack.Clear();
        // 레드칩 초기화
        saveData.InGameSaveData.redChipSaveDatas.Clear();
    }

    private void Start()
    {
        SoundManager.PlayBGM(SoundManager.SoundData_UI.LobbyBGM);
    }

    IEnumerator MoveMove()
    {
        while (!loadingObject.IsUnLoading())
        {
            yield return null;
        }
        loadingObject.StartLoading(saveData.InGameSaveData.chapterSaveData.Chapter);
    }
}
