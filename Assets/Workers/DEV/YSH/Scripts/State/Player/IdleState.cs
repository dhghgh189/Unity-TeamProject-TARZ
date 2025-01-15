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

        // fall 조건
        if (!owner.Movement.IsGrounded)
        {
            owner.ChangeState(EState.Fall);
            return;
        }

        // 대쉬
        if (owner.PInput.TryDash && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        if (owner.PInput.TryDrain && owner.Stat.CurrentStamina > 0)
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

    public override void OnFixedUpdate()
    {
        // idle에서는 velocity를 zero로 하여 미끄러지는 일이 없도록 함
        owner.Movement.Move(Vector3.zero);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
