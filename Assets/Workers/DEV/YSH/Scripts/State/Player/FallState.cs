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
    }

    public override void OnUpdate()
    {
        if (owner.PInput.TryThrow)
        {
            owner.ChangeState(EState.Throw);
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
