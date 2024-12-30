using UnityEngine;
using Zenject;

public class Example_StaminaScript : MonoBehaviour
{
    [Inject]
    [SerializeField] StatModel model;
    [SerializeField] float ee;

    public void Script()
    {
        if (model.CurrentStamina <= 0.0f) return;

        Debug.Log("스테미나 감소");
        model.ChangeStamina(-ee);
    }
}
