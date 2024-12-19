using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    void Activate(GameObject attacker, GameObject target);
}
