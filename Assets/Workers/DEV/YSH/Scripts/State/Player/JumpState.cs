using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState<PlayerController>
{
    private Vector3 moveDir;
    private float jumpCheckTimer;

    public JumpState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Jump;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (!owner.Movement.IsGrounded)
        {
            owner.ChangeState(EState.Idle);
            return;
        }
        owner.Anim.CrossFade(Define.HASH_ANIM_JUMP, 0.125f);
        owner.Movement.Jump(owner.Stat.JumpForce);

        jumpCheckTimer = 0.2f;
    }

    public override void OnUpdate()
    {
        if (owner.PInput.TryThrow)
        {
            owner.ChangeState(EState.Throw);
            return;
        }

        if (jumpCheckTimer > 0)
        {
            jumpCheckTimer -= Time.deltaTime;
            return;
        }

        if (owner.Movement.CurrentVelocity.y < 0)
        {
            owner.ChangeState(EState.Fall);
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
