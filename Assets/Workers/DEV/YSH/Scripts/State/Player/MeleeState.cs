using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeState : BaseState<PlayerController>
{
    private int[] meleeAnimHashes;

    private float animTimer;
    private float comboTimer;

    private Transform camTrf;

    private Vector3 lookDir;

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

    // 근접공격 애니메이션 재생
    public override void OnEnter()
    {
        if (camTrf == null)
            camTrf = Camera.main.transform;

        // Attack 시작 시에는 콤보 진행 불가능하도록 set
        owner.Attack.CanUseCombo = false;

        comboTimer = 0f;
        owner.Movement.Move(Vector3.zero);

        animTimer = 999;

        // 카메라 정면을 바라본다.
        lookDir = camTrf.forward;   // 공격 시전 시 바라봤던 방향을 기억해둔다.
        owner.Movement.LookAt(lookDir);

        owner.Anim.CrossFade(meleeAnimHashes[owner.Attack.MeleeCount], 0.01f);
        owner.StartCoroutine(AnimRoutine());
    }

    IEnumerator AnimRoutine()
    {
        // 애니메이션 재생 후 바로 info를 가져오면 이전 클립 정보가 받아지므로
        // 잠시 대기하는 시간을 가져야 한다.
        yield return new WaitForSeconds(0.1f);
        animTimer = owner.GetCurrentAnimTime();
    }

    public override void OnUpdate()
    {
        // 회전이 원복되는 현상 방지 (Adjust)
        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            owner.Movement.LookAt(lookDir);
        }

        // 대쉬가 입력되면 공격을 캔슬 (점프 시에는 불가)
        if (owner.Movement.IsGrounded
            && owner.PInput.TryDash
            && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.Attack.MeleeCount = 0;
            owner.ChangeState(EState.Dash);
            return;
        }

        // 콤보가 가능한 상황에 입력이 확인된 경우 
        if (owner.Attack.CanUseCombo && owner.PInput.TryMelee)
        {
            // 애니메이션이 끝나기 전에 전이하므로 카운트를 수동으로 증가
            owner.Attack.MeleeCount++;
            OnEnter();
            return;
        }

        // 애니메이션 재생이 완료되면 상태 종료
        if (animTimer <= 0)
        {
            // Adjust를 중지하기 위해 lookDir을 초기화
            lookDir = Vector3.zero;

            // 타수 초기화
            owner.Attack.MeleeCount = 0;
            owner.ChangeState(EState.Idle);
            return;
        }

        // timer 진행
        animTimer -= Time.deltaTime;
    }
}
