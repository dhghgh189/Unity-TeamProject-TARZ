using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSlot : MonoBehaviour
{
    [Inject] Loading LoadingObject;
    [SerializeField] int slotIndex;
    private SaveSlotData saveSlotData;

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
        path = Application.persistentDataPath + $"/SaveSlot{slotIndex}";
        if (!File.Exists(path))
        {
            saveSlotData = new();
            startButton.GetComponentInChildren<TMP_Text>().text = "NewGame";
        }
        else
        {
            saveSlotData = JsonUtility.FromJson<SaveSlotData>(File.ReadAllText(path));
            startButton.GetComponentInChildren<TMP_Text>().text = new DateTime(saveSlotData.CreateTime).ToString();
            Debug.Log(JsonUtility.ToJson(saveSlotData, true));
        }
        saveSlotData.SlotPath = path;
    }

    private void GameStart()
    {
        if (!File.Exists(path))
            saveSlotData.CreateTime = DateTime.Now.Ticks;
        FindAnyObjectByType<ProjectInstaller>().SaveSlotBind(saveSlotData);
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
