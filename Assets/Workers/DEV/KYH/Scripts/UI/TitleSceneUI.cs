using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] private Button titleButton;

    [Header("Input Manager")]
    [SerializeField] private ChangeInput inputManager;

    [Header("Load Game")]
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private LoadSceneUI loadScene;

    [Header("Settings")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private SettingSceneUI settingScene;

    private void Start()
    {
        inputManager.StartButton = titleButton;
        inputManager.firstInput = inputManager.StartButton;
        inputManager.firstInput.Select();
    }

    public void NewGameStart()
    {
        SceneManager.LoadScene("GameScene");    // 바로 새 게임 씬으로 이동(게임 씬 이름 변경 필요)
    }

    public void LoadGameStart()
    {
        gameObject.SetActive(false);
        loadGamePanel.SetActive(true);
    }

    public void OpenSettingsPanel()
    {
        gameObject.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
