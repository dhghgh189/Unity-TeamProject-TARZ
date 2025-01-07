using UnityEngine;
using Zenject;

public class Merchant : MonoBehaviour, Interaction_Ibase_Activate
{
    [SerializeField] UI_Merchant.EMerchant merchant;
    [Inject] UI_Merchant uI_Merchant;

    public void Activate()
    {
        uI_Merchant.Temp(merchant);
    }
}
