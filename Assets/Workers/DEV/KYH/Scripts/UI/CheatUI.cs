using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CheatUI : MonoBehaviour
{
    [Inject] StatModel model;

    public void MujeokMode(bool isOn)
    {
        Debug.Log(isOn);
        CheatManager.isMujeok = isOn;
    }

    public void ChipPlease()
    {
        model.Chip += 1000000f;
        model.BlackChip += 1000000f;
    }
}
