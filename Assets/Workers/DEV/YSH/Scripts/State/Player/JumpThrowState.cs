using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThrowState : BaseState<PlayerController>
{
    private Transform camTrf;
    private Vector3 lookDir;

    private Coroutine throwRoutine;

    public JumpThrowState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.JumpThrow;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (camTrf == null)
            camTrf = Camera.main.transform;

        owner.Movement.Move(Vector3.zero);

        // 카메라 정면을 바라본다.
        lookDir = camTrf.forward;   // 공격 시전 시 바라봤던 방향을 기억해둔다.
        owner.Movement.LookAt(lookDir);

        throwRoutine = owner.StartCoroutine(ThrowRoutine());
    }

    IEnumerator ThrowRoutine()
    {
        for(int i = 0; i < owner.Attack.JumpThrowAmount; i++)
        {
            owner.Attack.JumpThrow();
            yield return Util.GetDelay(owner.Attack.JumpThrowInterval);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            owner.Movement.LookAt(lookDir);
        }

        // 대쉬가 입력되면 공격을 캔슬
        if (owner.PInput.TryDash
            && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            Stop();
            owner.ChangeState(EState.Dash);
            return;
        }

        if (owner.Movement.IsGrounded)
        {
            Stop();
            owner.Movement.Move(Vector3.zero);
            owner.ChangeState(EState.Idle);
            return;
        }
    }

    public void Stop()
    {
        owner.StopCoroutine(throwRoutine);
        throwRoutine = null;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
