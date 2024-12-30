using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatUI : MonoBehaviour
{
    public void MujeokMode(bool isOn)
    {
        Debug.Log(isOn);
        CheatManager.isMujeok = isOn;
    }
}
