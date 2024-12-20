using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSceneUI : MonoBehaviour
{
    [SerializeField] private Button settingsButton;

    [Header("Input Manager")]
    [SerializeField] private ChangeInput inputManager;

    void Start()
    {
        inputManager.StartButton = settingsButton;
        inputManager.firstInput = inputManager.StartButton;
        inputManager.firstInput.Select();
    }

    void Update()
    {
        
    }
}
