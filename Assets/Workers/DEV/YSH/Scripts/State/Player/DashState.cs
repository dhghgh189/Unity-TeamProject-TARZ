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

        // 대쉬 시 입력값이 있는지 확인하기 위해 저장
        moveDir = owner.PInput.InputDir.normalized;

        // 방향키 입력이 있는 경우 카메라 정면을 기준으로 입력한 방향을 바라본다.
        if (moveDir != Vector3.zero)
        {
            owner.Movement.LookAt(moveDir);
        }
        // 방향키 입력이 없는 경우 카메라 정면을 바라본다.
        else
        {
            moveDir = Vector3.forward;
            owner.Movement.LookAt(camTrf.forward);
        }

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

        // 지정된 방향으로 대쉬 진행
        owner.Movement.Move(moveDir * owner.Stat.DashSpeed);
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.Movement.Move(Vector3.zero);
    }
}
