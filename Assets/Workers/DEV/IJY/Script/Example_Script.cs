using UnityEngine;
using Zenject;

public class Example_Script : MonoBehaviour
{
    [Inject] [SerializeField] StatModel model;
    [Inject] [SerializeField] PlayerController player;
    [SerializeField] float staminaUsing;
    [SerializeField] float damageUsing;


    public void Script()
    {
        if (model.CurrentStamina <= 0.0f) return;

        Debug.Log("스테미나 감소");
        model.ChangeStamina(-staminaUsing);
    }

    public void Script_()
    {
        model.CurrentMp = model.MaxMp;
    }

    public void Script__()
    {
        player.GetComponent<IDamagable>().TakeDamage(damageUsing);
    }
}
