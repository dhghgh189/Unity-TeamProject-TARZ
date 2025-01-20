using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSlot : MonoBehaviour
{
    [Inject] Loading LoadingObject;

    // 세이브 슬롯의 넘버링 고유한 번호를 가져야 함
    [SerializeField] int slotIndex;

    private SaveSlotData saveSlotData = new();
    private Button startButton;
    private Button resetButton;

    private string path;

    private void Awake()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(GameStart);

        resetButton = GetComponentsInChildren<Button>()[1];
        resetButton.onClick.AddListener(ResetSlot);
    }
    private void Start()
    {
        // 저장 경로에 파일이 있다면 역 직렬화를 통해 saveSlotData에 저장함
        path = Application.persistentDataPath + $"/SaveSlot{slotIndex}";
        if (!File.Exists(path))
        {
            startButton.GetComponentInChildren<TMP_Text>().text = "NewGame";
        }
        else
        {
            saveSlotData = JsonUtility.FromJson<SaveSlotData>(File.ReadAllText(path));
            startButton.GetComponentInChildren<TMP_Text>().text = new DateTime(saveSlotData.CreateTime).ToString();
        }
        // 세이브 매니저를 통한 저장을 위해 경로 저장
        saveSlotData.SlotPath = path;
    }

    private void GameStart()
    {
        // 저장 파일이 없다면 생성
        if (!File.Exists(path))
        {
            saveSlotData.CreateTime = DateTime.Now.Ticks;
            File.WriteAllText(path, JsonUtility.ToJson(saveSlotData));
        }

        // saveSlotData을 프로젝트에 바인딩 
        FindAnyObjectByType<ProjectInstaller>().SaveSlotBind(saveSlotData);
        Debug.Log(JsonUtility.ToJson(saveSlotData.InGameSaveData, true));
        LoadingObject.StartLoading(Define.SceneType.Lobby);
        gameObject.SetActive(false);
    }

    private void ResetSlot()
    {
        File.Delete(path);

        saveSlotData = new();
        saveSlotData.SlotPath = path;
        startButton.GetComponentInChildren<TMP_Text>().text = "NewGame";
    }
}
