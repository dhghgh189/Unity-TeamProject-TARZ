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
        Debug.Log(10 * statModel.DefaultPowerPer);
    }
    public void SkillAtack()
    {
        Debug.Log(10 * statModel.SkillPowerPer);
    }
    public void ElementalAtack()
    {
        Debug.Log(10 * statModel.ElementalPowerPer);
    }
}
