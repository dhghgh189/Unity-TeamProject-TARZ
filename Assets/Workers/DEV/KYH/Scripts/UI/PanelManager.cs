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

    [SerializeField] private PlayerController player;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Title")
        {
            player = null;
        }
        else
        {
            player = FindAnyObjectByType<PlayerController>();
            if (player == null)
            {
                Debug.LogWarning("PlayerController가 없당!");
            }
        }
    }

    private void Update()
    {
        isActive = CheckActivePanel();

        if (SceneManager.GetActiveScene().name == "Title") return;

        if (player == null)
        {
            Debug.LogError("PlayerController가 초기화되지 않았습니다.");
            return;
        }

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
                // 타이틀 씬에서는 player가 null이므로 IsCanControl 설정을 건너뜀
                if (SceneManager.GetActiveScene().name != "Title" && player != null)
                {
                    player.PInput.IsCanControl = false;
                }

                Time.timeScale = 0;
                return true;
            }
        }

        // 패널이 모두 비활성화된 경우
        if (SceneManager.GetActiveScene().name == "Title")
        {
            Time.timeScale = 1;
            return false;
        }
        else
        {
            // player가 null일 경우를 방어
            if (player != null)
            {
                player.PInput.IsCanControl = true;
            }
            Time.timeScale = 1;
            return false;
        }
    }
}
