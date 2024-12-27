using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveSlotsMenu : MonoBehaviour
{
    [Inject] private SaveManager manager;

    [SerializeField] private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void ActivateMenu()
    {
        //Dictionary<string, SaveData> profilesSaveData = 
    }
}
