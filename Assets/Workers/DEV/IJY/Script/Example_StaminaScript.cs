using UnityEngine;
using Zenject;

public class Example_StaminaScript : MonoBehaviour
{
    [Inject]
    [SerializeField] StatModel model;
    [SerializeField] float ee;
    [SerializeField] float gg;

    public void Script()
    {
        if (model.CurrentStamina <= 0.0f) return;

        Debug.Log("스테미나 감소");
        model.ChangeStamina(-ee);
    }

    public void Script_()
    {
        model.MoveSpeed = gg;
    }
}
