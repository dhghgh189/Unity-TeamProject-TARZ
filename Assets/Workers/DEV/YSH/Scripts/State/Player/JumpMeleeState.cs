using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMeleeState : BaseState<PlayerController>
{
    private Transform camTrf;
    private Vector3 lookDir;
    private int jumpMeleeAnimHash;

    public JumpMeleeState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.JumpMelee;

        jumpMeleeAnimHash = Animator.StringToHash("JumpMelee");
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (camTrf == null)
            camTrf = Camera.main.transform;

        // 카메라 정면을 바라본다.
        lookDir = camTrf.forward;   // 공격 시전 시 바라봤던 방향을 기억해둔다.
        owner.Movement.LookAt(lookDir);

        owner.SkillHandler.Use(SkillEnum.ActTimingType.Attack);
        owner.Anim.CrossFade(jumpMeleeAnimHash, 0.01f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (owner.Movement.IsGrounded)
        {
            Debug.Log("<color=cyan>Jump Melee Attack!!</color>");
            owner.Movement.Move(Vector3.zero);
            owner.ChangeState(EState.Idle);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
