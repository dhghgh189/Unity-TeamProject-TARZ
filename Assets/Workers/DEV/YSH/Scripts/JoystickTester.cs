using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoystickTester : MonoBehaviour
{
    [SerializeField] TMP_Text txtJoystickName;
    [SerializeField] TMP_Text[] txtAxies;
    [SerializeField] TMP_Text[] txtButtons;

    private void Update()
    {
        txtJoystickName.text = Input.GetJoystickNames()[0];

        for (int i = 0; i < txtAxies.Length; i++)
        {
            txtAxies[i].text = $"Axis {i + 1} : {Input.GetAxisRaw($"Axis {i + 1}")}";
        }

        for (int i = 0; i < txtButtons.Length; i++)
        {
            txtButtons[i].text = $"Button {i + 1} : {Input.GetAxisRaw($"Button {i + 1}")}";
        }
    }
}
