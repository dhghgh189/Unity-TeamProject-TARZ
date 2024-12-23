using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;         // 타이틀 패널
    [SerializeField] private GameObject activeCPanel;       // 현재 활성화 중인 패널

    [Header("<color=yellow>Input Manager</color>")]
    [SerializeField] private ChangeInput inputManager;      // UI 네비게이션 InputManager 참조용

    [Header("<color=orange>Category Buttons</color>")]
    [SerializeField] private Button gameplayButton;         // 게임 플레이 카테고리 버튼
    [SerializeField] private Button langueButton;           // 언어 카테고리 버튼
    [SerializeField] private Button soundButton;            // 사운드 카테고리 버튼
    [SerializeField] private Button keySettingsButton;      // 키 설정 카테고리 버튼

    [Header("<color=purple>Category Panels</color>")]
    [SerializeField] private GameObject nonSelectPanel;     // 설정 패널 첫 활성화 때 나오는 빈 패널
    [SerializeField] private GameObject gameplayPanel;      // 게임 플레이 카테고리 패널
    [SerializeField] private GameObject languagePanel;      // 언어 카테고리 패널
    [SerializeField] private GameObject soundPanel;         // 사운드 카테고리 패널
    [SerializeField] private GameObject keySettingsPanel;   // 키 설정 카테고리 패널

    [Header("<color=blue>Selected UI</color>")]
    [SerializeField] private Toggle activeMinimapToggle;    // 미니맵 활성화 체크 토글
    [SerializeField] private TMP_Dropdown languageDropdown; // 언어 선택 드롭다운 (참조가 되지 않는 오류로 주석처리)
    [SerializeField] private Slider masterVolumeSlider;     // 마스터 볼륨 조절 슬라이더

    private void Start()
    {
        activeCPanel = nonSelectPanel;                  // 현재 활성화 중인 패널을 nonSelectPanel로 설정
        inputManager.firstInput = gameplayButton;       // 설정 패널의 UI 네비게이션 첫 Input을 gameplayButton로 설정
        inputManager.firstInput.Select();               // 첫 Input으로 지정한 오브젝트를 선택 처리
    }

    private void OnEnable()
    {
        activeCPanel = nonSelectPanel;                  // 현재 활성화 중인 패널을 nonSelectPanel로 설정
        inputManager.firstInput = gameplayButton;       // 설정 패널의 UI 네비게이션 첫 Input을 gameplayButton로 설정
        inputManager.firstInput.Select();               // 첫 Input으로 지정한 오브젝트를 선택 처리
    }

    private void Update()
    {
        // ESC(컨트롤러 B버튼) 입력 시 행동
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            // 현재 활성화 중인 카테고리 패널에 맞는 카테고리 버튼 선택
            if (activeCPanel == gameplayPanel)
            {
                gameplayButton.Select();
            }
            else if (activeCPanel == languagePanel)
            {
                langueButton.Select();
            }
            else if (activeCPanel == soundPanel)
            {
                soundButton.Select();
            }
            else if (activeCPanel == keySettingsPanel)
            {
                keySettingsButton.Select();
            }
        }
    }

    // 게임 플레이 카테고리 버튼 클릭
    public void OnClickGamePlayButton()
    {
        activeCPanel = gameplayPanel;           // 현재 활성화 중인 패널을 게임 플레이 카테고리 패널로 설정
        nonSelectPanel.SetActive(false);        // 게임 플레이 카테고리 패널을 제외하고 모두 비활성화
        gameplayPanel.SetActive(true);
        languagePanel.SetActive(false);
        soundPanel.SetActive(false);
        keySettingsPanel.SetActive(false);
        activeMinimapToggle.Select();           // activeMinimapToggle 오브젝트를 UI 네비게이션 Input 시작으로 선택
    }

    // 언어 카테고리 버튼 클릭
    // 언어 카테고리 버튼 클릭 기능에서 드롭다운 UI의 참조 오류가 있어 현재는 제대로 기능하지 않음.
    public void OnClickLanguageButton()
    {
        activeCPanel = languagePanel;           // 현재 활성화 중인 패널을 언어 카테고리 패널로 설정
        nonSelectPanel.SetActive(false);        // 언어 카테고리 패널을 제외하고 모두 비활성화
        gameplayPanel.SetActive(false);
        languagePanel.SetActive(true);
        soundPanel.SetActive(false);
        keySettingsPanel.SetActive(false);
        languageDropdown.Select();              // languageDropdown 오브젝트를 UI 네비게이션 Input 시작으로 선택
    }

    // 사운드 카테고리 버튼 클릭
    public void OnClickSoundButton()
    {
        activeCPanel = soundPanel;              // 현재 활성화 중인 패널을 사운드 카테고리 패널로 설정
        nonSelectPanel.SetActive(false);        // 사운드 카테고리 패널을 제외하고 모두 비활성화
        gameplayPanel.SetActive(false);
        languagePanel.SetActive(false);
        soundPanel.SetActive(true);
        keySettingsPanel.SetActive(false);
        masterVolumeSlider.Select();            // masterVolumeSlider 오브젝트를 UI 네비게이션 Input 시작으로 선택
    }

    // 키 설정 카테고리 버튼 클릭
    public void OnClickKeySettingsButton()
    {
        activeCPanel = soundPanel;              // 현재 활성화 중인 패널을 키 설정 카테고리 패널로 설정
        nonSelectPanel.SetActive(false);        // 키 설정 카테고리 패널을 제외하고 모두 비활성화
        gameplayPanel.SetActive(false);
        languagePanel.SetActive(false);
        soundPanel.SetActive(false);
        keySettingsPanel.SetActive(true);
        // TODO : 키세팅 패널 시작 UI Select
    }

    // 타이틀 화면으로 돌아가기 버튼 클릭
    public void OnClickBackToTitleButton()
    {
        gameObject.SetActive(false);            // 설정 패널 비활성화
        titlePanel.SetActive(true);             // 타이틀 패널 활성화
    }
}
