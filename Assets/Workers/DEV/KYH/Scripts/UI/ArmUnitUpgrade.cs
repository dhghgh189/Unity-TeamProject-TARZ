using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ArmUnitUpgrade : MonoBehaviour
{
    [Inject] StatModel statModel;

    [SerializeField] private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void OnEnable()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void OnClickAttack_01Button()
    {
        statModel.SetAbility(AdditionAbility.AllPowerPer, 10);
        button.interactable = false;
    }

    public void OnClickAttack_02Button()
    {
        statModel.SetAbility(AdditionAbility.AllPowerPer, 20);
        button.interactable = false;
    }

    public void OnClickAttack_03Button()
    {
        statModel.SetAbility(AdditionAbility.AllPowerPer, 30);
        button.interactable = false;
    }
}
