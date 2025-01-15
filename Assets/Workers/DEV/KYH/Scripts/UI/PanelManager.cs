using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
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
                        Time.timeScale = 1f;
                    }
                }
            }
        }
    }

    private bool CheckActivePanel()
    {
        AudioClip panelOn = SoundManager.SoundData_UI.OnUI;
        AudioClip panelOff = SoundManager.SoundData_UI.OffUI;

        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                Time.timeScale = 0f;
                SoundManager.PlaySFX(panelOn);
                return true;
            }
        }
        Time.timeScale = 1f;
        SoundManager.PlaySFX(panelOff);
        return false;
    }
}
