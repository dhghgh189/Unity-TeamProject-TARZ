using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeState : BaseState<PlayerController>
{
    private int[] meleeAnimHashes;

    private float animTimer;
    private float comboTimer;

    private int meleeCount;

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
        meleeCount = owner.Attack.MeleeCount;

        comboTimer = 0f;
        owner.Movement.Move(Vector3.zero);

        animTimer = 999;

        owner.Anim.CrossFade(meleeAnimHashes[owner.Attack.MeleeCount], 0.01f);
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
        // 애니메이션 재생이 완료되면 상태 종료
        if (animTimer <= 0)
        {
            if (meleeCount >= owner.Attack.MeleeCountMax-1)
            {
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
                owner.Attack.MeleeCount = 0;
                owner.ChangeState(EState.Idle);
                return;
            }

            if (owner.PInput.TryMelee)
            {
                OnEnter();
                return;
            }
        }
    }
}
