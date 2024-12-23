using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KnockBack 이펙트 클래스
/// IKnockBack을 구현한 객체로부터 KnockBack을 호출한다.
/// </summary>
public class KnockBack : IEffect
{
    public void Activate(GameObject attacker, GameObject target)
    {
        Debug.Log("<color=red>Activate Knock Back</color>");
        IKnockBack knockBackable = target.GetComponent<IKnockBack>();
        knockBackable?.KnockBack(attacker);
    }
}
