using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject cheatPanel;

    bool isLocked;

    private void Start()
    {
        isLocked = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Cursor.visible = isLocked;
            Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
            isLocked = !isLocked;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            cheatPanel.SetActive(!cheatPanel.activeSelf);
        }
    }
}
