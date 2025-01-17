using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 가방 스킬 사용 상태
/// </summary>
public class UseBagState : BaseState<PlayerController>
{
    // 해당 능력의 사용이  끝났는지 확인하는 변수
    public bool IsEnd { get => owner.BagSkillHandler.ActionEnd; }
    private float excpetionTimer;

    public UseBagState(PlayerController owner)
    {
        this.owner = owner; type = EState.BagUse;
    }

    public override void OnEnter()
    {
        excpetionTimer = 0f;
        base.OnEnter();
        owner.BagSkillHandler.CurNode.Value.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        excpetionTimer += Time.deltaTime;

        if (excpetionTimer >= 5f)
        {
            owner.ChangeState(EState.Idle);
            return;
        }

        if (owner.PInput.TryDash && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        owner.BagSkillHandler.CurNode.Value.OnUpdate();

        if (IsEnd)
        {
            owner.ChangeState(EState.Idle);
        }
    }

    public override void OnFixedUpdate()
    {
        owner.BagSkillHandler.CurNode.Value.OnFixedUpdate();
    }

    public override void OnExit()
    {
        owner.BagSkillHandler.CurNode.Value.OnExit();
        owner.BagSkillHandler.End();
        base.OnExit();
    }

}
