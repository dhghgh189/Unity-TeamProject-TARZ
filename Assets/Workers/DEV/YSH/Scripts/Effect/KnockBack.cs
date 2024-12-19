using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : IEffect
{
    public void Activate(GameObject attacker, GameObject target)
    {
        Debug.Log("Activate Knock Back");
        IKnockBack knockBackable = target.GetComponent<IKnockBack>();
        knockBackable?.KnockBack(attacker);
    }
}
