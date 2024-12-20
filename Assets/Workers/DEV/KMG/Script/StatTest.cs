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
    public void DefaultAtack()
    {
        Debug.Log(statModel.DefaultPower);
    }
    public void SkillAtack()
    {
        Debug.Log(statModel.SkillPower);
    }
    public void ElementalAtack()
    {
        Debug.Log(statModel.ElementalPower);
    }
}
