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
        Debug.Log(statModel.DefaultPowerPer);
    }
    public void SkillAtack()
    {
        Debug.Log(statModel.SkillPowerPer);
    }
    public void ElementalAtack()
    {
        Debug.Log(statModel.ElementalPowerPer);
    }
}
