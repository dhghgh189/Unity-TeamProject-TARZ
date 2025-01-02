/*using TMPro;
using UnityEngine;
using Zenject;

public class SaveSlot : MonoBehaviour
{
    [Inject] private SaveManager manager;

    [SerializeField] private string profileId = "";
    [SerializeField] private GameObject slotButton;
    [SerializeField] private TMP_Text slotText;

    public void SetData(SaveData data)
    {
        if (data == null)
        {
            slotText.text = "Empty";
        }
        else
        {
            slotText.text = "Save File";
            slotText.text = $"{profileId}";
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public void OnSaveSlotButtonClick()
    {
        if (PlayerPrefs.HasKey(profileId))
        {
            Debug.Log($"<color=yellow>{profileId}에서 데이터 로드");
            Load();
        }
        else
        {
            Debug.Log($"<color=red>{profileId}에 데이터 저장");
            manager.Save();
        }
    }

    public void Load()
    {
        string saveData = PlayerPrefs.GetString(profileId);
        PlayerPrefs.SetString("SaveData", saveData);
        Debug.Log($"<color=green>{profileId} 로드 완료!</color>");
    }

   *//* private void UpdateSlotUI()
    {
        if (PlayerPrefs.HasKey(profileId))
        {
            string saveJson = PlayerPrefs.GetString(profileId);
            SaveData saveData = JsonUtility.FromJson<SaveData>(saveJson);
            SetData(saveData);
        }
        else
        {
            SetData(null);
        }
    }*//*

    public System.DateTime? GetLastSavedTime()
    {
        if (PlayerPrefs.HasKey(profileId + "_LastSavedTime"))
        {
            string timeText = PlayerPrefs.GetString(profileId + "_LastSavedTime");

            if (System.DateTime.TryParse(timeText, out var lastSavedTime))
            {
                return lastSavedTime;
            }
        }
        return null;
    }
}
*/