using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestPlayerUI : MonoBehaviour
{
    [Inject] StatModel model;

    public void OnClickHPButton()
    {
        model.CurrentHp -= 10f;
    }

    public void OnClickMPButton()
    {
        model.CurrentMp += 0.1f;
    }

    public void OnClickStaminaButton()
    {
        model.CurrentStamina -= 2f;
    }
}
