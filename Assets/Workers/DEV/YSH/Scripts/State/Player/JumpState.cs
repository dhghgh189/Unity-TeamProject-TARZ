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
        owner.Movement.Jump(owner.Movement.JumpForce);

        jumpCheckTimer = 0.2f;
    }

    public override void OnUpdate()
    {
        // 현재 물건 스택이 점프 공격에 필요한 스택만큼 존재해야 함
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
