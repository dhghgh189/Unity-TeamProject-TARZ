using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class UI_Merchant : MonoBehaviour
{
    public enum EMerchant { Hp, Stat, Chip, Equipment }

    [SerializeField] GameObject hpMerchantPanel;
    [SerializeField] GameObject statMerchantPanel;
    [SerializeField] GameObject chipMerchantPanel;
    [SerializeField] GameObject equipmentMerchantPanel;

    public void Temp(EMerchant merchant)
    {
        hpMerchantPanel.SetActive(merchant == EMerchant.Hp && !hpMerchantPanel.activeSelf);
        statMerchantPanel.SetActive(merchant == EMerchant.Stat && !statMerchantPanel.activeSelf);
        chipMerchantPanel.SetActive(merchant == EMerchant.Chip && !chipMerchantPanel.activeSelf);
        equipmentMerchantPanel.SetActive(merchant == EMerchant.Equipment && !equipmentMerchantPanel.activeSelf);

        OnlyInteractAction(hpMerchantPanel.activeSelf || statMerchantPanel.activeSelf || chipMerchantPanel.activeSelf || equipmentMerchantPanel.activeSelf);
    }
    private void OnlyInteractAction(bool on)
    {
        InputActionMap actionMap = InputSystem.actions.FindActionMap("Player");
        if (on)
        {
            foreach (var action in actionMap.actions)
            {
                if (action.name != "Interact")
                {
                    action.Disable();
                }
            }
        }
        else
        {
            actionMap.Enable();
        }
    }
}
