using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : IEffect
{
    public void Activate(GameObject attacker, GameObject target)
    {
        Debug.Log("<color=red>Activate Knock Back</color>");
        IKnockBack knockBackable = target.GetComponent<IKnockBack>();
        knockBackable?.KnockBack(attacker);
    }
}
