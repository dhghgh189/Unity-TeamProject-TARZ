using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private bool isActive = false;

    private PlayerController player;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            player = null;
        }
        else
        {
            player = FindAnyObjectByType<PlayerController>();
        }
    }

    private void Update()
    {
        isActive = CheckActivePanel();

        if (SceneManager.GetActiveScene().name == "Title") return;

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
                        player.PInput.IsCanControl = true;
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
                player.PInput.IsCanControl = false;
                Time.timeScale = 0;
                return true;
            }
        }

        if (SceneManager.GetActiveScene().name == "Title")
        {
            Time.timeScale = 1;
            return false;
        }
        else
        {
            player.PInput.IsCanControl = true;
            Time.timeScale = 1;
            return false;
        }
    }
}
