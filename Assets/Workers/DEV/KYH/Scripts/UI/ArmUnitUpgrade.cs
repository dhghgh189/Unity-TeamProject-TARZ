using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ArmUnitUpgrade : MonoBehaviour
{
    [Inject] StatModel statModel;

    public void OnClickAttack_01Button()
    {
        statModel.SetAbility(AdditionAbility.AllPowerPer, 10);
    }

    public void OnClickAttack_02Button()
    {
        statModel.SetAbility(AdditionAbility.AllPowerPer, 20);
    }

    public void OnClickAttack_03Button()
    {
        statModel.SetAbility(AdditionAbility.AllPowerPer, 30);
    }
}
