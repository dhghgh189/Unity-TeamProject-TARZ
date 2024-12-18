using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : BaseState<PlayerController>
{
    private int[] meleeAnimHashes;

    private float animTimer;

    public MeleeState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Melee;

        meleeAnimHashes = new int[owner.Attack.MeleeCountMax];

        for (int i = 0; i < meleeAnimHashes.Length; i++)
        {
            meleeAnimHashes[i] = Animator.StringToHash($"Melee{i + 1}");
        }
    }

    // �������� �ִϸ��̼� ���
    public override void OnEnter()
    {
        owner.Movement.Move(Vector3.zero);

        animTimer = 999;
        owner.Anim.CrossFade(meleeAnimHashes[owner.Attack.MeleeCount], 0.01f);
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
        // �ִϸ��̼� ����� �Ϸ�Ǹ� ���� ����
        if (animTimer <= 0)
        {
            owner.ChangeState(EState.Idle);
            return;
        }

        // timer ����
        animTimer -= Time.deltaTime;
    }
}
