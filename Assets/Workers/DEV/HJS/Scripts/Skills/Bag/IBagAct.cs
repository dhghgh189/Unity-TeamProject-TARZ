using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBagAct
{
    public float CurGauge { get; set; }
    public PlayerController player { get; set; }

    public LinkedList<BaseBagState> Acts { get; set; }

    /// <summary>
    /// 사용할 수 있는지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    public bool IsCanUse();
    /// <summary>
    /// 게이지 충전 함수
    /// </summary>
    public void Charge();
    /// <summary>
    /// 게이지 소모 함수
    /// </summary>
    public void Use();

    /// <summary>
    /// 능력을 사용하고 복원해야하는 함수
    /// </summary>
    public void RetrunFeature();
}
