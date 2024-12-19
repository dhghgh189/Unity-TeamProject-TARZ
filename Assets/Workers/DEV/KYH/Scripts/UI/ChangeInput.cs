using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeInput : MonoBehaviour
{
    // UI Selectable Input 시작점으로 지정할 버튼UI
    [SerializeField] private Button startButton;

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

        // 컨트롤러 왼쪽 조이스틱을 위로(== 키보드 위 방향키) 입력할 떄
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 다음 선택될 UI 오브젝트를 UI 네비게이션에서 현재 선택된 UI 오브젝트의 이전 오브젝트로 지정
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();

            // 다음 선택될 UI 오브젝트가 null이 아닌 경우 선택
            if (next != null)
            {
                next.Select();
            }
        }
        // 컨트롤러 왼쪽 조이스틱을 아래로(== 키보드아래 방향키) 입력할 떄
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 다음 선택될 UI 오브젝트를 UI 네비게이션에서 현재 선택된 UI 오브젝트의 다음 오브젝트로 지정
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            // 다음 선택될 UI 오브젝트가 null이 아닌 경우 선택
            if (next != null)
            {
                next.Select();
            }
        }
    }
}
