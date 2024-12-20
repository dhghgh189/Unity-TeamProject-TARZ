using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] private Button titleButton;

    [Header("<color=yellow>Input Manager</color>")]
    [SerializeField] private ChangeInput inputManager;

    [Header("<color=orange>Load Game</color>")]
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private LoadSceneUI loadScene;

    [Header("<color=green>Settings</color>")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private SettingSceneUI settingScene;

    private void Start()
    {
        inputManager.firstInput = titleButton;
        inputManager.firstInput.Select();
    }

    private void OnEnable()
    {
        inputManager.firstInput = titleButton;
        inputManager.firstInput.Select();
    }

    public void OnClickNewGameButton()
    {
        SceneManager.LoadScene("GameScene");    // 바로 새 게임 씬으로 이동(게임 씬 이름 변경 필요)
    }

    public void OnClickLoadGameButton()
    {
        gameObject.SetActive(false);
        loadGamePanel.SetActive(true);
    }

    public void OnClickSettingsButton()
    {
        gameObject.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnClickQuitGameButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
