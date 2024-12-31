using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject keyboardPanel;
    [SerializeField] private GameObject gamepadPanel;

    public void OnClickKeyboardButton()
    {
        keyboardPanel.SetActive(true);
        gamepadPanel.SetActive(!keyboardPanel.activeSelf);
    }

    public void OnClickGamepadButton()
    {
        gamepadPanel.SetActive(true);
        keyboardPanel.SetActive(!gamepadPanel.activeSelf);
    }
}
