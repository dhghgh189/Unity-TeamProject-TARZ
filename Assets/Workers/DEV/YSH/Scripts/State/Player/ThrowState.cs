using System.Collections;
using System.Linq;
using UnityEngine;

public class ThrowState : BaseState<PlayerController>
{
    private int[] throwAnimHashes;          // 단일 액션에 대한 애니메이션 해쉬
    private int[,] throwMultiAnimHashes;    // 멀티 액션에 대한 애니메이션 해쉬

    private float animTimer;

    private float comboTimer;

    private Vector3 inputDir;

    private Transform camTrf;

    private Vector3 lookDir;

    public ThrowState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Throw;

        throwAnimHashes = new int[owner.Attack.ThrowCountMax];
        throwMultiAnimHashes = new int[owner.Attack.ThrowCountMax, (int)EMultiActionType.Length];

        for (int i = 0; i < throwAnimHashes.Length; i++)
        {
            throwAnimHashes[i] = Animator.StringToHash($"Throw{i + 1}");

            // Multi Action 애니메이션 Hash 생성
            if (owner.Attack.ThrowAttackInfo[i].MultiActions.Length > 0)
            {
                MultiActionInfo[] multiActions = owner.Attack.ThrowAttackInfo[i].MultiActions;
                for (int j = 0; j < multiActions.Length; j++)
                {
                    // actionType enum을 index로 사용
                    int iActionType = (int)multiActions[j].ActionType;
                    throwMultiAnimHashes[i, iActionType] = Animator.StringToHash($"Throw{i + 1}_{multiActions[j].ActionType}");
                }
            }
        }
    }

    public override void OnEnter()
    {
        if (owner.interactioner.SpecialOBJ != null)
        {
            owner.ChangeState(EState.Idle);
            return;
        }

        if (camTrf == null)
            camTrf = Camera.main.transform;

        // Attack 시작 시에는 콤보 진행 불가능하도록 set
        owner.Attack.CanUseCombo = false;
        // 공격 시작시에는 움직이지 못하도록 set
        owner.Attack.CanMoveWhileAttack = false;

        // 최초 진입시점 때의 입력값을 기억한다.
        inputDir = owner.PInput.InputDir;

        comboTimer = 0f;

        // 아직 anim length를 모르기 때문에 큰 값으로 설정
        animTimer = 999;

        // 동작 처리
        if (!SetAction())
        {
            // 조건이 맞지않아 공격 진행에 실패하면 상태 초기화
            owner.ChangeState(EState.Idle);        
            return;
        }

        owner.Movement.Move(Vector3.zero);

        // 카메라 정면을 바라본다.
        lookDir = camTrf.forward;   // 공격 시전 시 바라봤던 방향을 기억해둔다.
        owner.Movement.LookAt(lookDir);

        owner.StartCoroutine(AnimRoutine());
    }

    private bool SetAction()
    {
        MultiActionInfo[] multiActions = owner.Attack.ThrowAttackInfo[owner.Attack.ThrowCount].MultiActions;
        if (multiActions.Length <= 0)
        {
            owner.Anim.CrossFade(throwAnimHashes[owner.Attack.ThrowCount], 0.01f);
            return true;
        }

        int animHash;

        // 패드 지원을 위해 추가 작업 진행
        float xValue = Mathf.Abs(inputDir.x);
        float zValue = Mathf.Abs(inputDir.z);

        if (inputDir == Vector3.zero)   // Basic Type
        {
            animHash = throwMultiAnimHashes[owner.Attack.ThrowCount, (int)EMultiActionType.Basic];
            owner.Attack.ActionType = EMultiActionType.Basic;
        }
        else if (xValue >= zValue)      // Horizontal Type
        {
            animHash = throwMultiAnimHashes[owner.Attack.ThrowCount, (int)EMultiActionType.Horizontal];
            owner.Attack.ActionType = EMultiActionType.Horizontal;
        }
        else                            // Vertical Type
        {
            animHash = throwMultiAnimHashes[owner.Attack.ThrowCount, (int)EMultiActionType.Vertical];
            owner.Attack.ActionType = EMultiActionType.Vertical;
        }

        // 물건 스택 체크
        MultiActionInfo currentAction = multiActions.Where(x => x.ActionType == owner.Attack.ActionType).First();
        if (owner.Attack.ObjectCount < currentAction.StackAmount)
        {
            Debug.Log("<color=red>물건이 부족합니다!!</color>");
            return false;
        }

        owner.Anim.CrossFade(animHash, 0.01f);
        return true;
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
        // 애니메이션 도중 회전이 원복되는 현상 방지 (Adjust)
        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            owner.Movement.LookAt(lookDir);
        }

        // 대쉬가 입력되면 공격을 캔슬
        if (owner.PInput.TryDash
            && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        // 콤보 입력 종료 후 유저가 이동을 입력한 경우 이동으로 캔슬
        if (owner.Attack.CanMoveWhileAttack && owner.PInput.InputDir != Vector3.zero)
        {
            owner.ChangeState(EState.Move);
            return;
        }

        // 콤보가 가능한 상황에 입력이 확인된 경우 
        // 물건 스택또한 존재해야 함
        if (owner.Attack.CanUseCombo
            && owner.PInput.TryThrow
            && owner.Attack.ObjectCount > 0)
        {
            // 애니메이션이 끝나기 전에 전이하므로 카운트를 수동으로 증가
            owner.Attack.ThrowCount++;
            OnEnter();
            return;
        }

        // 애니메이션 재생이 완료되면 상태 종료
        if (animTimer <= 0)
        {
            // Adjust를 중지하기 위해 lookDir을 초기화
            lookDir = Vector3.zero;

            owner.ChangeState(EState.Idle);
            return;
        }

        // timer 진행
        animTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.Attack.ThrowCount = 0;
    }
}
