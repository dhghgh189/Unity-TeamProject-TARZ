using BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject activeCPanel;

    [Header("<color=yellow>Input Manager</color>")]
    [SerializeField] private ChangeInput inputManager;

    [Header("<color=orange>Category Buttons</color>")]
    [SerializeField] private Button gameplayButton;
    [SerializeField] private Button langueButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button keySettingsButton;

    [Header("<color=purple>Category Panels</color>")]
    [SerializeField] private GameObject nonSelectPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject languagePanel;
    [SerializeField] private GameObject soundPanel;

    [Header("<color=blue>Selected UI</color>")]
    [SerializeField] private Toggle activeMinimapToggle;
    //[SerializeField] private Dropdown languageDropdown;
    [SerializeField] private Slider masterVolumeSlider;

    private void Start()
    {
        activeCPanel = nonSelectPanel;
        inputManager.firstInput = gameplayButton;
        inputManager.firstInput.Select();
    }

    private void OnEnable()
    {
        activeCPanel = nonSelectPanel;
        inputManager.firstInput = gameplayButton;
        inputManager.firstInput.Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (activeCPanel == gameplayPanel)
            {
                gameplayButton.Select();
            }
            else if (activeCPanel = languagePanel)
            {
                langueButton.Select();
            }
            else if (activeCPanel = soundPanel)
            {
                soundButton.Select();
            }
        }
    }

    public void OnClickGamePlayButton()
    {
        activeCPanel = gameplayPanel;
        nonSelectPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        languagePanel.SetActive(false);
        soundPanel.SetActive(false);
        activeMinimapToggle.Select();
    }

    public void OnClickLanguageButton()
    {
        activeCPanel = languagePanel;
        nonSelectPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        languagePanel.SetActive(true);
        soundPanel.SetActive(false);
        //languageDropdown.Select();
    }

    public void OnClickSoundButton()
    {
        activeCPanel = soundPanel;
        nonSelectPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        languagePanel.SetActive(false);
        soundPanel.SetActive(true);
        masterVolumeSlider.Select();
    }

    public void OnClickBackToTitleButton()
    {
        gameObject.SetActive(false);
        titlePanel.SetActive(true);
    }
}
