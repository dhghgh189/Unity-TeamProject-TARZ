using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KnockBack 이펙트를 구현하기 위한 인터페이스
/// 해당 인터페이스를 상속받아 실제 동작을 구현
/// </summary>
public interface IKnockBack
{
    void KnockBack(GameObject attacker);
}
