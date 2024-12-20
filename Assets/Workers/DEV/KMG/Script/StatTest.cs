using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StatTest : MonoBehaviour
{
    [Inject] StatModel statModel;
    public void PlayerHit(float value)
    {
        statModel.CurrentHp -= value;
    }
}
