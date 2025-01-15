using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PanelManager : MonoBehaviour
{
    //[Inject] PlayerController player;

    [SerializeField] private GameObject[] panels;
    [SerializeField] private bool isActive = false;

    private void Update()
    {
        isActive = CheckActivePanel();

        if (isActive)
        {
            foreach (GameObject panel in panels)
            {
                if (!panel.activeSelf) continue;

                foreach (GameObject other in panels)
                {
                    if (other != panel && other.activeSelf)
                    {
                        other.SetActive(false);
                        //player.PInput.IsCanControl = true;
                        Time.timeScale = 1;
                    }
                }
            }
        }
    }

    private bool CheckActivePanel()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                //player.PInput.IsCanControl = false;
                Time.timeScale = 0;
                return true;
            }
        }
        //player.PInput.IsCanControl = true;
        Time.timeScale = 1;
        return false;
    }
}
