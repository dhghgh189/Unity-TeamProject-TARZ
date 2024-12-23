using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 Effect가 구현해야 하는 인터페이스
/// </summary>
public interface IEffect
{
    void Activate(GameObject attacker, GameObject target);
}
