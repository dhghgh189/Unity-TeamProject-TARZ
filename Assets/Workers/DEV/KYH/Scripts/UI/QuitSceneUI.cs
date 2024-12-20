using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitSceneUI : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] Button noButton;

    [Header("<color=yellow>Input Manager</color>")]
    [SerializeField] private ChangeInput inputManager;          // UI 네비게이션 InputManager 참조용

    private void OnEnable()
    {
        inputManager.firstInput = noButton;       // 타이틀 패널의 UI 네비게이션 첫 Input을 newGameButton로 설정
        inputManager.firstInput.Select();         // 첫 Input으로 지정한 오브젝트를 선택 처리
    }

    // No 버튼 클릭
    public void OnClickNoButton()
    {
        gameObject.SetActive(false);
        titlePanel.SetActive(true);
    }

    // Yes 버튼 클릭
    public void OnClickYesButton()
    {
        // Unity Editor일 경우, 에디터 플레이 모드 종료
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        // Unity Editor가 아닐 경우, 어플리케이션 종료
        #else
                Application.Quit();
        #endif
    }
}
