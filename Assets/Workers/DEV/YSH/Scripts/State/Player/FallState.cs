using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BaseState<PlayerController>
{
    private Vector3 moveDir;

    public FallState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Fall;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.Anim.CrossFade(Define.HASH_ANIM_FALL, 0.125f);
    }

    public override void OnUpdate()
    {
        // 대쉬가 입력되면 캔슬
        if (owner.PInput.TryDash
            && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        if (owner.PInput.TryThrow
            && owner.Attack.ObjectCount >= owner.Attack.JumpThrowAmount)
        {
            // 점프 원거리 공격
            owner.ChangeState(EState.JumpThrow);
            return;
        }

        if (owner.PInput.TryMelee)
        {
            // 점프 근거리 공격
            owner.ChangeState(EState.JumpMelee);
            return;
        }

        if (owner.Movement.IsGrounded)
        {
            owner.Movement.Move(Vector3.zero);
            owner.ChangeState(EState.Idle);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        moveDir = owner.PInput.InputDir.normalized;
        owner.Movement.Move(moveDir * owner.Stat.MoveSpeed);
    }
}
