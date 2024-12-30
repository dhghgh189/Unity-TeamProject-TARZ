using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuPanel : MonoBehaviour
{
    [Inject] SaveManager saveManager;
    //[Inject] SaveSlot saveSlot;
    private InputAction menuAction;
    private bool isActive;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private ChangeInput inputManager;
    [SerializeField] private Button selectButton;

    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        menuAction = InputSystem.actions.FindAction("Menu");
        inputManager.firstInput = selectButton;
        inputManager.firstInput.Select();
    }

    private void OnEnable()
    {
        inputManager.firstInput = selectButton;
        inputManager.firstInput.Select();
    }

    private void Update()
    {
        if (menuAction.WasPressedThisFrame())
        {
            if (isActive)
            {
                menuPanel.SetActive(false);
                isActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                menuPanel.SetActive(true);
                isActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void OnClickSettingsButton()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void OnClickBackToGameButton()
    {
        menuPanel.SetActive(false);
    }

    public void OnClickBackToMenuButton()
    {
        settingsPanel.SetActive(false);
    }

    public void OnClickQuitGameButton()
    {
        //if (saveSlot != null)
        //{
        //    saveManager.Save();
        //}

        SceneManager.LoadScene(0);
    }
}
