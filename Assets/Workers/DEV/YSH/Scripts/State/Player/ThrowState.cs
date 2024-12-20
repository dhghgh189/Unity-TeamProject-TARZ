using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : BaseState<PlayerController>
{
    private int[] throwAnimHashes;
    private float animTimer;

    private float comboTimer;
    private int throwCount;

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
        throwCount = owner.Attack.ThrowCount;

        comboTimer = 0f;
        owner.Movement.Move(Vector3.zero);

        // 아직 anim length를 모르기 때문에 큰 값으로 설정
        animTimer = 999;

        owner.Anim.CrossFade(throwAnimHashes[owner.Attack.ThrowCount], 0.01f);
        owner.StartCoroutine(AnimRoutine());
    }

    IEnumerator AnimRoutine()
    {
        // 애니메이션 재생 후 바로 info를 가져오면 이전 클립 정보가 받아지므로
        // 잠시 대기하는 시간을 가져야 한다.
        yield return new WaitForSeconds(0.1f);
        AnimatorStateInfo info = owner.Anim.GetCurrentAnimatorStateInfo(0);
        // 현재 재생된 애니메이션의 length를 받는다 (speed가 고려되야 함)
        animTimer = info.length / info.speed;
    }

    public override void OnUpdate()
    {
        // 대쉬가 입력되면 공격을 캔슬 (점프 시에는 불가)
        // 공격 카운트도 체크하여 마지막 공격때는 캔슬안되게 해야 함
        if (owner.Movement.IsGrounded && owner.PInput.TryDash)
        {
            owner.Attack.ThrowCount = 0;
            owner.ChangeState(EState.Dash);
            return;
        }

        // 애니메이션 재생이 완료되면 상태 종료
        if (animTimer <= 0)
        {
            // 마지막 타수였거나 물건 스택이 없는 경우 바로 Idle로 이동
            if (throwCount >= owner.Attack.ThrowCountMax - 1 
                || owner.Attack.ObjectCount <= 0)
            {
                owner.Attack.ThrowCount = 0;
                owner.ChangeState(EState.Idle);
            }
            else
            {
                comboTimer = owner.Attack.ComboCheckTime;
                animTimer = 999;
            }

            return;
        }

        // timer 진행
        animTimer -= Time.deltaTime;

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            // 다음 콤보를 사용하기 까지 제한시간
            if (comboTimer <= 0)
            {
                owner.Attack.ThrowCount = 0;
                owner.ChangeState(EState.Idle);
                return;
            }

            if (owner.PInput.TryThrow)
            {
                OnEnter();
                return;
            }
        }
    }
}
