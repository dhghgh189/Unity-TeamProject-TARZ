using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
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
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }
}
