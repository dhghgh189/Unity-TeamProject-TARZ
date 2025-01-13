using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventoryButton : MonoBehaviour
{
    [Inject] ChangeInput input;

    [SerializeField] Button inventoryButton;
    [SerializeField] Button bluechipButton;

    [SerializeField] GameObject inventory;
    [SerializeField] GameObject bluechip;

    private void Start()
    {
        inventoryButton.onClick.AddListener(OnClickInventoryButton);
        bluechipButton.onClick.AddListener(OnClickBluechipButton);
    }

    public void OnClickInventoryButton()
    {
        inventory.SetActive(true);
        bluechip.SetActive(false);
    }

    public void OnClickBluechipButton()
    {
        inventory.SetActive(false);
        bluechip.SetActive(true);
    }

    private void OnDestroy()
    {        
        inventoryButton.onClick.RemoveListener(OnClickInventoryButton);
        bluechipButton.onClick.RemoveListener(OnClickBluechipButton);
    }
}
