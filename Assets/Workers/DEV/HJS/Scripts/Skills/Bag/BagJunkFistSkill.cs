using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;
using static BagSkillEnum;
/// <summary>
/// 가방 스킬 - 정크 피스트
/// </summary>
public class BagJunkFistSkill : IBagAct
{
    /* 식별자 */
    private readonly BagIndexKey keyName = BagIndexKey.JunkFist;
    /* 기본 정보들 */
    private BagSkillDataSO skilldata;
    private float maxGauge;
    private float chargeAmount;
    private float useAmount;
    private float curGauge;
    private PlayerController owner;

    private LinkedList<BaseBagState> acts;
    public LinkedList<BaseBagState> Acts { get => acts; set { } }
    public PlayerController player { get => owner; set { owner = value; } }

    /* 특수 정보들 */
    public int ResultDamage;
    public MeleeAttackInfo[] MeleeAttackInfo;
    public MeleeAttackInfo[] temp;

    public BagJunkFistSkill(PlayerController owner, BagSkillContainerSO container)
    {
        try
        {
            // 없을 경우에만 넣기
            if(skilldata == null)
            {
                //데이터 셋에서 keyName에 해당하는 데이터 가져오기
                skilldata = container.GetData(keyName);
            }
        }
        catch(Exception e) { Debug.LogException(e); } 
        finally
        {
            Debug.Log("일단 불러보기 성공!");
        }

        maxGauge = skilldata.MaxGauge;
        chargeAmount = skilldata.ChargeAmount;
        useAmount = skilldata.UseAmount;
        curGauge = 0;

        this.owner = owner;

        acts = new LinkedList<BaseBagState>();
        acts.AddLast(new BagJunkFist_1(owner, this));

        // 기존의 공격 모션 담아두기
        temp = owner.Attack.MeleeAttackInfo;
        // 변경할 공격 모션 담아두기
        MeleeAttackInfo = new MeleeAttackInfo[] {  };
    }

    // 게이지 충전
    public void Charge() => curGauge = (curGauge + chargeAmount >= maxGauge) ? maxGauge : curGauge + chargeAmount;
    //게이지 사용 가능한지 확인
    public bool IsCanUse() => curGauge >= useAmount;
    // 게이지 사용
    public void Use() => curGauge -= useAmount;
    // 특수 능력
    public void Feature()
    {
        // 2. 파편 계산 - Damage 체크
        ResultDamage = owner.Attack.ObjectCount / 10;
        int removeCount = ResultDamage * 3;
        Debug.Log($"<color=green>데미지 증가량 : {ResultDamage}</color>");
        owner.Attack.RemoveThrowObject(removeCount);
        owner.Stat.ExtraDamage += ResultDamage;
        // 3. 특수 기믹 발동 -> 모든 공격 상태 진입 x -> 30초간
        owner.StartCoroutine(BuffRoutine());
        // 4. 기본 근접 공격의 방식을 변경
        //  owner.Attack.MeleeAttackInfo = MeleeAttackInfo;
        //  owner.Attack.GenerateMeleeEffects();
    }

    public void RetrunFeature()
    {
        // TODO: 변경할 데이터
        // 전체 -> 상태 전이 막아놓거 풀기
        owner.StateTransfer.OnEnableState(new EState[]{ EState.Throw, EState.JumpThrow, EState.Melee, EState.JumpMelee});
        owner.Stat.ExtraDamage -= ResultDamage;
    }

    private IEnumerator BuffRoutine()
    {
        // 버프
        owner.StateTransfer.OnDisableState(new EState[] { EState.Throw, EState.JumpThrow, EState.Melee, EState.JumpMelee });
        owner.Stat.ExtraDamage += ResultDamage;
        Use();
        Debug.Log("버프 작동!");
        yield return Util.GetDelay(5f);    // 필요 데이터 - 지속 시간

        // 정상 종료
        owner.StateTransfer.OnEnableState(new EState[] { EState.Throw, EState.JumpThrow, EState.Melee, EState.JumpMelee });
        owner.Stat.ExtraDamage -= ResultDamage;
        Debug.Log("버프 끝!");
    }

    /// <summary>
    /// 1번 동작: 로봇 팔 생성
    /// </summary>
    public class BagJunkFist_1 : BaseBagState
    {
        private BagJunkFistSkill parent;
        private string animName = "Drain";

        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        private float animTimer;

        public BagJunkFist_1(PlayerController owner, BagJunkFistSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("자 시작해 버렸다");

            owner.Movement.Rigid.velocity = Vector3.zero;

            animTimer = 999f;

            if (camTrf == null)
                camTrf = Camera.main.transform;

            moveDir = owner.PInput.InputDir.normalized;

            if (moveDir != Vector3.zero)
            {
                owner.Movement.LookAt((camTrf.right * moveDir.x) + (camTrf.forward * moveDir.z));
            }
            // 방향키 입력이 없는 경우 카메라 정면을 바라본다.
            else
            {
                owner.Movement.LookAt(camTrf.forward);
            }

            lookDir = owner.transform.forward;

            owner.Anim.CrossFade(Animator.StringToHash(animName), 0.01f);
            owner.StartCoroutine(AnimRoutine());
        }

        private IEnumerator AnimRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            animTimer = owner.GetCurrentAnimTime();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Debug.Log("자 업데이트해 버렸다");

            if (animTimer <= 0)
            {
                // TODO: 근접 공격외 공격 요소 봉인 + 공격 모션 변경
                parent.Feature();

                owner.BagSkillHandler.NextStep();
            }
            animTimer -= Time.deltaTime;
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("자 끝나버렸다");
        }
    }
}
