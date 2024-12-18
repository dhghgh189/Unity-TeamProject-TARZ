using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : BaseState<PlayerController>
{
    // attack ��ũ��Ʈ ������ ���°� ������?
    private int[] throwAnimHashes;

    // �ִϸ��̼� ��� �Ϸ� üũ�� ���� timer
    private float animTimer;

    public ThrowState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Throw;

        throwAnimHashes = new int[owner.Attack.ThrowCountMax];

        for (int i = 0; i < throwAnimHashes.Length; i++)
        {
            throwAnimHashes[i] = Animator.StringToHash($"Throw{i + 1}");
        }
    }

    public override void OnEnter()
    {
        owner.Movement.Move(Vector3.zero);

        // ���� anim length�� �𸣱� ������ ū ������ ����
        animTimer = 999;

        owner.Anim.CrossFade(throwAnimHashes[owner.Attack.ThrowCount], 0.01f);
        owner.StartCoroutine(AnimRoutine());
    }

    IEnumerator AnimRoutine()
    {
        // �ִϸ��̼� ��� �� �ٷ� info�� �������� ���� Ŭ�� ������ �޾����Ƿ�
        // ��� ����ϴ� �ð��� ������ �Ѵ�.
        yield return new WaitForSeconds(0.1f);
        AnimatorStateInfo info = owner.Anim.GetCurrentAnimatorStateInfo(0);
        // ���� ����� �ִϸ��̼��� length�� �޴´� (speed�� ����Ǿ� ��)
        animTimer = info.length / info.speed;
    }

    public override void OnUpdate()
    {
        // �뽬�� �ԷµǸ� ������ ĵ�� (���� �ÿ��� �Ұ�)
        // ���� ī��Ʈ�� üũ�Ͽ� ������ ���ݶ��� ĵ���ȵǰ� �ؾ� ��
        if (owner.Movement.IsGrounded && owner.PInput.TryDash)
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        // �ִϸ��̼� ����� �Ϸ�Ǹ� ���� ����
        if (animTimer <= 0)
        {
            if (!owner.Movement.IsGrounded)
                owner.ChangeState(EState.Fall);
            else
                owner.ChangeState(EState.Idle);

            return;
        }

        // timer ����
        animTimer -= Time.deltaTime;
    }
}
