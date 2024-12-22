using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private Vector3 moveDir;

    private float dashTimer;
    private Transform camTrf;

    Vector3 lookDir;

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
            owner.Movement.LookAt((camTrf.right * moveDir.x) + (camTrf.forward * moveDir.z));
        }
        // 방향키 입력이 없는 경우 카메라 정면을 바라본다.
        else
        {
            owner.Movement.LookAt(camTrf.forward);
        }

        // 대쉬 진행시작 방향을 기억한다.
        lookDir = owner.transform.forward;

        owner.Anim.CrossFade(Define.HASH_ANIM_DASH, 0.1f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 회전이 원복되는 현상 방지 (Adjust)
        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            Debug.Log("<color=red>Adjust forward</color>");
            owner.Movement.LookAt(lookDir);
        }

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
        {
            owner.ChangeState(EState.Idle);
            return;
        }

        // 대쉬의 경우 진행중 카메라 방향의 영향을 받지 않아야 하므로 Move 함수 사용 없이 velocity만 변경
        // 기억해뒀던 방향 (lookDir) 으로 진행
        owner.Movement.Rigid.velocity = lookDir * owner.Stat.DashSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.Movement.Move(Vector3.zero);
    }
}
