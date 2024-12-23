using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class exScript : MonoBehaviour
{
    [Inject]
    [SerializeField] StatModel model;
    [SerializeField] float ee;

    public void Script()
    {
        if (model.CurrentStamina <= 0.0f) return;

        Debug.Log("스테미나 감소");
        model.CurrentStamina -= ee;
    }
}
