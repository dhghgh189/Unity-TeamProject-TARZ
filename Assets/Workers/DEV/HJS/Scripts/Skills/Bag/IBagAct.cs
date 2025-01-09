using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBagAct
{
    public LinkedList<BaseBagState> Acts { get; set; }

    public bool IsCanUse();
    // 게이지 충전 함수
    public void Charge();
    // 게이지 소모 함수
    public void Use();
}
