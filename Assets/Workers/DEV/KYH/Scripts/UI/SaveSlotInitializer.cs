using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveSlotInitializer : MonoBehaviour
{
    [Inject] private SaveManager manager;

    [SerializeField] private SaveSlot[] saveSlots;

    private void Start()
    {
        foreach (SaveSlot slot in saveSlots)
        {
            string profileId = slot.GetProfileId();

            if (PlayerPrefs.HasKey(profileId))
            {
                string saveJson = PlayerPrefs.GetString(profileId);
                SaveData saveData = JsonUtility.FromJson<SaveData>(saveJson);
                slot.SetData(saveData);
            }
            else
            {
                slot.SetData(null);
            }
        }
    }
}