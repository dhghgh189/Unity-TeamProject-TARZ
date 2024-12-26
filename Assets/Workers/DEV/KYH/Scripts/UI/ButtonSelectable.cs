using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.VirtualTexturing;
using TMPro;

public class ButtonSelectable : MonoBehaviour
{
    [SerializeField] private ChangeInput inputManager;
    [SerializeField] private Button button;

    [Header("Text")]
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDes;
    [SerializeField] private TMP_Text skillCost;

    private EventSystem system;

    private void Start()
    {
        system = EventSystem.current;
        inputManager.firstInput = button;
        inputManager.firstInput.Select();
    }

    private void OnEnable()
    {
        system = EventSystem.current;
        inputManager.firstInput = button;
        inputManager.firstInput.Select();
    }

    private void Update()
    {
        ChangeText();
    }

    private void ChangeText()
    {
        if (system.currentSelectedGameObject == null) return;

        switch (system.currentSelectedGameObject.name)
        {
            case "Attack_01":
                skillName.text = "Add Attack 1";
                skillDes.text = "Add Attack 10%";
                break;

            case "Attack_02":
                skillName.text = "Add Attack 2";
                skillDes.text = "Add Attack 20%";
                break;

            case "Attack_03":
                skillName.text = "Add Attack 3";
                skillDes.text = "Add Attack 30%";
                break;
        }
    }
}
