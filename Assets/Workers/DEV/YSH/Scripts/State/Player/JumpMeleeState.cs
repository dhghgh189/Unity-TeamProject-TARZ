using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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

        owner.Movement.Rigid.velocity = Vector3.zero;

        // 카메라 정면을 바라본다.
        lookDir = camTrf.forward;   // 공격 시전 시 바라봤던 방향을 기억해둔다.
        owner.Movement.LookAt(lookDir);

        owner.SkillHandler.Use(SkillEnum.ActTimingType.Attack);
        owner.Anim.CrossFade(jumpMeleeAnimHash, 0.1f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            owner.Movement.LookAt(lookDir);
        }

        // 모든 점프 근거리 공격 처리가 끝났을 때 상태 종료
        if (owner.Attack.IsEndJumpMelee)
        {
            owner.ChangeState(EState.Idle);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        // 플레이어가 점프 근거리 공격으로 인한 하강 중일 때
        if (!owner.Attack.IsEndJumpMelee)
        {
            // 기본 하강속도보다 빠르게 하강하도록 하기 위해 AddForce
            owner.Movement.Rigid.AddForce(Vector3.down * 10f, ForceMode.Force);
        }
    }
}
