using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] private Button newGameButton;              // 타이틀 패널 시작 UI

    [Header("<color=yellow>Input Manager</color>")]
    [SerializeField] private ChangeInput inputManager;          // UI 네비게이션 InputManager 참조용

    [Header("<color=orange>Load Game</color>")]
    [SerializeField] private GameObject loadGamePanel;          // 저장된 게임 불러오기 패널
    [SerializeField] private LoadSceneUI loadScene;             // 저장된 게임 불러오기 패널 클래스

    [Header("<color=green>Settings</color>")]
    [SerializeField] private GameObject settingsPanel;          // 설정 패널
    [SerializeField] private SettingSceneUI settingScene;       // 설정 패널 클래스

    [Header("<color=red>Quit Game</color>")]
    [SerializeField] private GameObject quitPanel;              // 게임 나가기 패널
    [SerializeField] private QuitSceneUI quitScene;             // 게임 나가기 패널 클래스

    private void Start()
    {
        inputManager.firstInput = newGameButton;  // 타이틀 패널의 UI 네비게이션 첫 Input을 newGameButton로 설정
        inputManager.firstInput.Select();         // 첫 Input으로 지정한 오브젝트를 선택 처리
    }

    private void OnEnable()
    {
        inputManager.firstInput = newGameButton;  // 타이틀 패널의 UI 네비게이션 첫 Input을 newGameButton로 설정
        inputManager.firstInput.Select();         // 첫 Input으로 지정한 오브젝트를 선택 처리
    }

    // 새로 시작 버튼 클릭
    public void OnClickNewGameButton()
    {
        SceneManager.LoadScene("GameScene");    // 바로 새 게임 씬으로 이동(게임 씬 이름 변경 필요)
    }

    // 저장된 게임 시작 버튼 클릭
    public void OnClickLoadGameButton()
    {
        gameObject.SetActive(false);    // 타이틀 패널 비활성화
        loadGamePanel.SetActive(true);  // 저장된 게임 불러오기 패널 활성화
    }

    // 설정 버튼 클릭
    public void OnClickSettingsButton()
    {
        gameObject.SetActive(false);    // 타이틀 패널 비활성화
        settingsPanel.SetActive(true);  // 설정 패널 활성화
    }

    // 게임 종료 버튼 클릭
    public void OnClickQuitGameButton()
    {
        gameObject.SetActive(false);    // 타이틀 패널 비활성화
        quitPanel.SetActive(true);      // 게임 나가기 패널 활성화
    }
}
