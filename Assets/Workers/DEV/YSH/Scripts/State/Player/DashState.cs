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
        // ����Ű�� �뽬�� ���� ���ȴ��� Ȯ���ϱ� ���� moveDir�� ����
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
        // ����Ű �Է°� �뽬�� ���� ���� ���
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
