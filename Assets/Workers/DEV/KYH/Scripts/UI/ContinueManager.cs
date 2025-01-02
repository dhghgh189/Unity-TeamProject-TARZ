/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ContinueManager : MonoBehaviour
{
    [Inject] SaveManager manager;
    [Inject] SaveSlot saveSlot;

    [SerializeField] SaveSlot[] saveSlots;
    [SerializeField] string defalutSceneName;

    private SaveSlot GetLatesSavedSlot()
    {
        SaveSlot latestSlot = null;
        System.DateTime? latestTime = null;

        foreach (var slot in saveSlots)
        {
            System.DateTime? slotTime = slot.GetLastSavedTime();

            if (slotTime.HasValue && (latestTime == null || slotTime > latestTime))
            {
                latestTime = slotTime;
                latestSlot = slot;
            }
        }

        if (saveSlot != null)
        {
            System.DateTime? saveTime = saveSlot.GetLastSavedTime();

            if (saveTime.HasValue && (latestTime == null || saveTime > latestTime))
            {
                latestSlot = saveSlot;
            }
        }

        return latestSlot;
    }

    public void OnClickSlotButton()
    {
        saveSlot.Load();
        SceneManager.LoadScene(1);
    }
}
*/