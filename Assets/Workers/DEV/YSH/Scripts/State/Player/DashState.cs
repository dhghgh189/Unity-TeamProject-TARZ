using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private Vector3 moveDir;
    private Vector3 velocity;

    private float dashTimer;
    private Transform camTrf;

    public DashState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Dash;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (camTrf == null)
            camTrf = Camera.main.transform;

        dashTimer = owner.Stat.DashTime;

        // 카메라 정면을 바라본다.
        owner.Movement.LookAt(camTrf.forward);
        owner.Anim.CrossFade(Define.HASH_ANIM_DASH, 0.1f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
        {
            owner.ChangeState(EState.Idle);
            return;
        }

        owner.Movement.Move(Vector3.forward * owner.Stat.DashSpeed);
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.Movement.Move(Vector3.zero);
    }
}
