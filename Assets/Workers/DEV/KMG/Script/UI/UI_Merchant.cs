using UnityEngine;
using Zenject;

public class UI_Merchant : MonoBehaviour
{
    public enum EMerchant { Hp, Stat, Chip, Equipment }
    
    [Inject] PlayerController playerController;

    [SerializeField] GameObject hpMerchantPanel;
    [SerializeField] GameObject statMerchantPanel;
    [SerializeField] GameObject chipMerchantPanel;
    [SerializeField] GameObject equipmentMerchantPanel;

    public void Temp(EMerchant merchant)
    {
        playerController.PInput.IsCanControl = !playerController.PInput.IsCanControl;
        hpMerchantPanel.SetActive(merchant == EMerchant.Hp && !hpMerchantPanel.activeSelf);
        statMerchantPanel.SetActive(merchant == EMerchant.Stat && !statMerchantPanel.activeSelf);
        chipMerchantPanel.SetActive(merchant == EMerchant.Chip && !chipMerchantPanel.activeSelf);
        equipmentMerchantPanel.SetActive(merchant == EMerchant.Equipment && !equipmentMerchantPanel.activeSelf);
    }
}
