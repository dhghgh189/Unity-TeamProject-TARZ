using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private Vector3 moveDir;
    private Vector3 velocity;

    private Coroutine dashRoutine;
    private float dashTimer;

    public DashState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Dash;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        dashTimer = owner.Stat.DashTime;
        // 방향키와 대쉬가 같이 눌렸는지 확인하기 위해 moveDir에 저장
        moveDir = owner.PInput.InputDir.normalized;
        dashRoutine = owner.StartCoroutine(DashRoutine());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (dashRoutine == null)
        {
            owner.ChangeState(EState.Idle);
            return;
        }
    }

    IEnumerator DashRoutine()
    {
        // 방향키 입력과 대쉬가 같이 눌린 경우
        if (moveDir != Vector3.zero)
            owner.Movement.LookAt(owner.PInput.InputDir.normalized);

        velocity = owner.transform.forward * owner.Stat.DashSpeed;

        owner.Anim.CrossFade(Define.HASH_ANIM_DASH, 0.1f);

        while (true)
        {
            if (dashTimer <= 0)
                break;

            owner.Movement.Move(velocity);
            dashTimer -= Time.deltaTime;

            yield return null;
        }

        owner.Movement.Move(Vector3.zero);
        dashRoutine = null;
    }
}
