using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadSceneUI : MonoBehaviour
{
    [Inject] ChangeInput inputManager;

    [SerializeField] private Button saveSlot_01;

    private void Start()
    {
        inputManager.firstInput = saveSlot_01;
        inputManager.firstInput.Select();
    }
}
