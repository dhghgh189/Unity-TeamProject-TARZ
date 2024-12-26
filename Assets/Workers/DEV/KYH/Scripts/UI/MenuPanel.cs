using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class MenuPanel : MonoBehaviour
{
    [Inject] SaveManager saveManager;

    [SerializeField] private ChangeInput inputManager;
    [SerializeField] private Button selectButton;

    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        inputManager.firstInput = selectButton;
        inputManager.firstInput.Select();
    }

    private void OnEnable()
    {
        inputManager.firstInput = selectButton;
        inputManager.firstInput.Select();
    }

    public void OnClickSettingsButton()
    {
        settingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickBackToGameButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickBackToMenuButton()
    {
        settingsPanel.SetActive(false);
    }

    public void OnClickQuitGameButton()
    {
        saveManager.Save();
        // TODO : 타이틀 화면으로 되돌아가기
    }
}
