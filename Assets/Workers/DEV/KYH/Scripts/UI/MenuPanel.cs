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
    [Inject] private ChangeInput inputManager;
    [Inject] PlayerController playerController;
    //[Inject] SaveSlot saveSlot;
    private InputAction menuAction;
    private bool isActive;

    [SerializeField] private GameObject menuPanel;      // 메뉴 패널 UI
    [SerializeField] private Button selectButton;       // 셀렉터블 선택 버튼
    [SerializeField] private GameObject settingsPanel;  // 설정 패널
    [SerializeField] private Button settingsButton;

    private void Start()
    {
        // 메뉴 키 입력 시 메뉴 불러오기
        menuAction = InputSystem.actions.FindAction("Menu");

        inputManager.firstInput = settingsButton;
        inputManager.firstInput.Select();
    }

    private void Update()
    {
        // 메뉴 키 입력 시 행동
        if (menuAction.WasPressedThisFrame())
        {
            // 메뉴 패널이 활성화 되어 있으면 메뉴 패널을 비활성화
            if (isActive)
            {
                SoundManager.PlaySFX(SoundManager.SoundData_UI.OffUI);
                ClosePanel();
            }
            // 메뉴 패널이 비활성화 되어 있으면 메뉴 패널을 활성화
            else
            {
                if (settingsPanel.activeSelf)
                    return;

                SoundManager.PlaySFX(SoundManager.SoundData_UI.OnUI);
                menuPanel.SetActive(true);
                isActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                playerController.PInput.IsCanControl = false;

                // 메뉴 패널 활성화 시 셀렉터블 UI 선택
                inputManager.firstInput = selectButton;
                inputManager.firstInput.Select();
            }
        }
    }

    /*private void OnDisable()
    {
        playerController.PInput.IsCanControl = true;
    }*/

    /// <summary>
    /// 설정 버튼 클릭
    /// </summary>
    public void OnClickSettingsButton()
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.OnUI);
        settingsPanel.SetActive(true);
        isActive = false;
        menuPanel.SetActive(false);
    }

    /// <summary>
    /// 게임으로 돌아가기 버튼 클릭
    /// </summary>
    public void OnClickBackToGameButton()
    {
        ClosePanel();
    }

    public void OnClickBackToMenuButton()
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.OffUI);
        settingsPanel.SetActive(false);
    }

    /// <summary>
    /// 게임 나가기 버튼 클릭
    /// </summary>
    public void OnClickQuitGameButton()
    {
        //if (saveSlot != null)
        //{
        //    saveManager.Save();
        //}

        SoundManager.PlaySFX(SoundManager.SoundData_UI.OffUI);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 패널 닫기 함수
    /// </summary>
    private void ClosePanel()
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.OffUI);
        menuPanel.SetActive(false);
        isActive = false;
        playerController.PInput.IsCanControl = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
