using System;
using System.Collections;
using System.Collections.Generic;
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

    /* 특수 정보들 */
    public float ResultDamage;

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
    }

    public void Charge()
    {
        // 게이지 충전
        curGauge = (curGauge + chargeAmount >= maxGauge) ? maxGauge : curGauge + chargeAmount;
    }
    
    public bool IsCanUse()
    {
        //게이지 사용 가능한지 확인
        return curGauge >= useAmount;
    }
    
    public void Use()
    {
        // 플로우에 따라 절차적으로 진행
        // 1.게이지 소모
        curGauge -= useAmount;
        // 2. 파편 계산 - Damage 체크
        int increaseDamage = owner.Attack.ObjectCount / 10;
        Debug.Log($"계산한 데미지 : {increaseDamage}");
        int removeCount = increaseDamage * 3;
        owner.Attack.RemoveThrowObject(removeCount);
        // 3. 특수 기믹 발동 -> 모든 공격 상태 진입 x -> 30초간
        // 이건 상태 갈 수 있냐 해주는 Bool 리스트를 변경하면 되는데
        // 4. 기본 근접 공격의 방식을 변경
        // 이게 진짜 대박임 -> 그러면 List를 교체하는 방식으로 ?
        // 무슨 방법인데 -> 뭐긴 뭐야 Attack 방식을 변경하고 애니메이션 방법도 변경해야지
    }

    /// <summary>
    /// 1번 동작: 로봇 팔 생성
    /// </summary>
    public class BagJunkFist_1 : BaseBagState
    {
        private BagJunkFistSkill parent;
        private string animName = "Idle";

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
                parent.Use();

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
