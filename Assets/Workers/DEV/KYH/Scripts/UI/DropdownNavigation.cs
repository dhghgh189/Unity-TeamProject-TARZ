using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropdownNavigation : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private GameObject firstOption;

    private void Start()
    {
        // 드롭다운 값 변경 이벤트 구독
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChanged(); });
    }

    /// <summary>
    /// 드롭다운 선택지가 있을 경우 선택지를 셀렉터블로 지정
    /// </summary>
    private void OnDropdownChanged()
    {
        if (EventSystem.current != null && firstOption != null)
        {
            EventSystem.current.SetSelectedGameObject(firstOption);
        }
    }
}
