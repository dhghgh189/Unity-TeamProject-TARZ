/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestPlayerUI : MonoBehaviour
{
    [Inject] StatModel model;

    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject menuPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            skillPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(true);
        }
    }

    public void OnClickHPButton()
    {
        model.CurrentHp -= 10f;
    }

    public void OnClickMPButton()
    {
        model.CurrentMp += 10f;
    }

    public void OnClickStaminaButton()
    {
        model.ChangeStamina(-2f);
    }
}
*/