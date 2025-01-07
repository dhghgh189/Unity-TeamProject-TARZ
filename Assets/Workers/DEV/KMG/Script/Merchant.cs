using UnityEngine;
using Zenject;

public class Merchant : MonoBehaviour, Interaction_Ibase_Activate
{
    [SerializeField] UI_Merchant.EMerchant merchant;
    private UI_Merchant uI_Merchant;

    private void Start()
    {
        uI_Merchant = FindAnyObjectByType<UI_Merchant>();
    }

    public void Activate()
    {
        uI_Merchant.Temp(merchant);
    }
}
