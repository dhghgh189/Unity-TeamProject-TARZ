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
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChanged(); });
    }

    private void OnDropdownChanged()
    {
        if (EventSystem.current != null && firstOption != null)
        {
            EventSystem.current.SetSelectedGameObject(firstOption);
        }
    }
}
