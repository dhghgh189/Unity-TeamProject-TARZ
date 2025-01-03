using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatUISelect : MonoBehaviour
{
    [SerializeField] ChangeInput inputManager;
    [SerializeField] Toggle mujeokToggle;

    private void OnEnable()
    {
        inputManager.firstInput = mujeokToggle;
        inputManager.firstInput.Select();
    }
}
