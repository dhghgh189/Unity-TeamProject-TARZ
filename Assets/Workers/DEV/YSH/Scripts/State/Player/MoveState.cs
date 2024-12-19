using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState<PlayerController>
{
    private Vector3 moveDir;

    public MoveState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Move;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.Anim.CrossFade(Define.HASH_ANIM_RUN, 0.125f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

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

        if (owner.PInput.TryThrow && owner.Attack.ObjectCount > 0)
        {
            owner.Movement.Move(Vector3.zero);
            owner.ChangeState(EState.Throw);
            return;
        }

        if (owner.PInput.TryMelee)
        {
            owner.Movement.Move(Vector3.zero);
            owner.ChangeState(EState.Melee);
            return;
        }

        if (owner.PInput.TryJump)
        {
            owner.ChangeState(EState.Jump);
            return;
        }

        if (owner.PInput.InputDir == Vector3.zero)
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

    public override void OnExit()
    {
        base.OnExit();
    }
}
