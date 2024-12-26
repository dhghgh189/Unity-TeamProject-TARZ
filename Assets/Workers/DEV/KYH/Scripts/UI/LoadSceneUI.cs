using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadSceneUI : MonoBehaviour
{
    [Inject] SaveManager saveManager;
    [Inject] SaveData saveData;
    [Inject] StatModel statModel;

    [ContextMenu("Load")]
    public void Load()
    {
        if (!PlayerPrefs.HasKey("SaveData"))
        {
            Debug.LogWarning("저장된 데이터가 없습니다.");
            return;
        }

        // PlayerPrefs에서 SaveData JSON 문자열 가져오기
        string jsonData = PlayerPrefs.GetString("SaveData");
        saveData = JsonUtility.FromJson<SaveData>(jsonData);
    }

    public void OnClickLoadGameButton()
    {
        Load();
        // TODO : 로비 씬으로 전환
    }
}
