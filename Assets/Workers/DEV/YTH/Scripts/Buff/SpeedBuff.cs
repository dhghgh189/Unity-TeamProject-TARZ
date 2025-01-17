using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    [SerializeField] private float percent;
    [SerializeField] private float waitTime;

    Coroutine rollbackRoutine;
    public override void Use(PlayerController player)
    {
        if (rollbackRoutine != null)
            return;

        player.Stat.SetAbility(AdditionAbility.MoveSpeedPer, percent);
        rollbackRoutine = StartCoroutine(RollbackRoutine(player));
        Debug.Log("<color=blue> 속도 버프 시작</color>");
    }

    IEnumerator RollbackRoutine(PlayerController player)
    {
        yield return Util.GetDelay(waitTime);
        player.Stat.SetAbility(AdditionAbility.MoveSpeedPer, -percent);
        Debug.Log("<color=blue> 속도 버프 종료</color>");
        rollbackRoutine = null;
    }
}
