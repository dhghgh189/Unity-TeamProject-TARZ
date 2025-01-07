using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] private Button newGameButton;              // 타이틀 패널 시작 UI

    [Header("<color=yellow>Input Manager</color>")]
    [SerializeField] private ChangeInput inputManager;          // UI 네비게이션 InputManager 참조용

    [Header("<color=orange>Load Game</color>")]
    [SerializeField] private GameObject loadGamePanel;          // 저장된 게임 불러오기 패널
    //[SerializeField] private LoadSceneUI loadScene;             // 저장된 게임 불러오기 패널 클래스

    [Header("<color=green>Settings</color>")]
    [SerializeField] private GameObject settingsPanel;          // 설정 패널
    [SerializeField] private SettingSceneUI settingScene;       // 설정 패널 클래스

    [Header("<color=red>Quit Game</color>")]
    [SerializeField] private GameObject quitPanel;              // 게임 나가기 패널
    [SerializeField] private QuitSceneUI quitScene;             // 게임 나가기 패널 클래스

    [Header("<color=white>Logo Panel</color>")]
    [SerializeField] private GameObject LogoPanel;

    [Header("<color=white>Loading Object</color>")]
    [Inject] private Loading LoadingObject;

    private Animator anim;
    private int fadeOutHash = Animator.StringToHash("Fade Out");
    private int fadeInHash = Animator.StringToHash("Fade In");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //inputManager.firstInput = newGameButton;  // 타이틀 패널의 UI 네비게이션 첫 Input을 newGameButton로 설정
        //inputManager.firstInput.Select();         // 첫 Input으로 지정한 오브젝트를 선택 처리

        
    }

    private void OnEnable()
    {
        if (!LogoPanel.gameObject.activeSelf)
        {
            inputManager.firstInput = newGameButton;  // 타이틀 패널의 UI 네비게이션 첫 Input을 newGameButton로 설정
            inputManager.firstInput.Select();         // 첫 Input으로 지정한 오브젝트를 선택 처리
        }
    }

    private void Update()
    {
        // 아무 키 입력 시 로고 패널을 Fade-Out으로 비활성화 처리
        if (Input.anyKeyDown && LogoPanel.gameObject.activeSelf)
        {
            anim.Play(fadeOutHash);
        }
    }

    // 게임 시작 버튼 클릭
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

    public void HideLogo()
    {
        LogoPanel.gameObject.SetActive(false);
        anim.Play(fadeInHash);
    }

    public void OnCompleteFadeIn()
    {
        inputManager.firstInput = newGameButton;  // 타이틀 패널의 UI 네비게이션 첫 Input을 newGameButton로 설정
        inputManager.firstInput.Select();         // 첫 Input으로 지정한 오브젝트를 선택 처리
    }
}
