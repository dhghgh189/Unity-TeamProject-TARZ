using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState<PlayerController>
{
    public IdleState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Idle;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.Anim.CrossFade(Define.HASH_ANIM_IDLE, 0.125f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        // 대쉬
        if (owner.PInput.TryDash)
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        if (owner.PInput.TryDrain)
        {
            owner.ChangeState(EState.Drain);
            return;
        }

        // 원거리 공격
        if (owner.PInput.TryThrow && owner.Attack.ObjectCount > 0)
        {
            owner.ChangeState(EState.Throw);
            return;
        }

        // 근접공격
        if (owner.PInput.TryMelee)
        {
            owner.ChangeState(EState.Melee);
            return;
        }

        // 이동
        if (owner.PInput.InputDir != Vector3.zero)
        {
            owner.ChangeState(EState.Move);
            return;
        }

        // 점프
        if (owner.PInput.TryJump)
        {
            owner.ChangeState(EState.Jump);
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
