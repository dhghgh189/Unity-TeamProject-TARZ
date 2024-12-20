using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeInput : MonoBehaviour
{
    // UI Selectable Input 시작점으로 지정할 버튼UI
    private Button startButton;
    public Button StartButton { get { return startButton; } set { startButton = value; } }

    private EventSystem system;         // 현재 씬의 UI 이벤트 시스템을 저장할 변수
    public Selectable firstInput;       // UI Selectable Input 시작점

    private void Start()
    {
        system = EventSystem.current;       // 현재 씬의 EventSystem을 system에 저장

        firstInput = startButton;           // UI Selectable Input 시작점을 startButton으로 지정
        firstInput.Select();                // UI Selectable Input 시작점을 선택
    }

    private void Update()
    {
        // 현재 선택된 UI 오브젝트가 없는 경우 예외 처리
        if (system.currentSelectedGameObject == null) return;

        // UI Selectable Input 변경은 기본 EventSystem과 UI 네비게이션 기능으로 구현되어 있음.
        // 키보드에서 W/S, 또는 방향키 위/아래 입력 시 이전/다음 UI 선택
        // 컨트롤러(게임패드)에서 왼쪽 조이스틱 위/아래 입력 시 이전/다음 UI 선택
    }
}
