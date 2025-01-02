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
    //[Inject] SaveSlot saveSlot;
    private InputAction menuAction;
    private bool isActive;

    [SerializeField] private GameObject menuPanel;      // 메뉴 패널 UI
    [SerializeField] private Button selectButton;       // 셀렉터블 선택 버튼
    [SerializeField] private GameObject settingsPanel;  // 설정 패널

    private void Start()
    {
        // 메뉴 키 입력 시 메뉴 불러오기
        menuAction = InputSystem.actions.FindAction("Menu");

        // 메뉴 패널 활성화 때 셀렉터블 UI 선택
        inputManager.firstInput = selectButton;
        inputManager.firstInput.Select();
    }

    private void OnEnable()
    {
        // 메뉴 패널 활성화 때 셀렉터블 UI 선택
        inputManager.firstInput = selectButton;
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
                menuPanel.SetActive(false);
                isActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            // 메뉴 패널이 비활성화 되어 있으면 메뉴 패널을 활성화
            else
            {
                menuPanel.SetActive(true);
                isActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    /// <summary>
    /// 설정 버튼 클릭
    /// </summary>
    public void OnClickSettingsButton()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    /// <summary>
    /// 게임으로 돌아가기 버튼 클릭
    /// </summary>
    public void OnClickBackToGameButton()
    {
        menuPanel.SetActive(false);
    }

    public void OnClickBackToMenuButton()
    {
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

        SceneManager.LoadScene(0);
    }
}
