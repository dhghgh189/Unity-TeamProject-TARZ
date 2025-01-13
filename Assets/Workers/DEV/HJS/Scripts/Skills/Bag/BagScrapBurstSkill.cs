using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BagSkillEnum;

/// <summary>
/// 가방 스킬 - 스크랩 버스트
/// </summary>
public class BagScrapBurstSkill : BagSkill
{
    /* 특수 정보들 */
    GameObject tmp;
    ScrapParentObject instance;

    public BagScrapBurstSkill(PlayerController owner, BagSkillContainerSO container)
    {
        keyName = BagIndexKey.ScrapBurst;
        try
        {
            // 없을 경우에만 넣기
            if (skilldata == null)
            {
                //데이터 셋에서 keyName에 해당하는 데이터 가져오기
                skilldata = container.GetData(keyName);
            }
        }
        catch (Exception e) { Debug.LogException(e); }
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
        acts.AddLast(new BagScrapBurst_1(owner, this));
        acts.AddLast(new BagScrapBurst_2(owner, this));

        tmp = Resources.Load("Managed/BagSkill/ScrapBrustObject") as GameObject;
    }

    /// <summary>
    /// 1번 동작 : 고철덩어리를 오른속에 쥔채 전방으로 팔을 뻗기
    /// </summary>
    public class BagScrapBurst_1 : BaseBagState
    {
        private BagScrapBurstSkill parent;
        private string animName = "ScrapBrust_1";

        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        private float animTimer;

        public BagScrapBurst_1(PlayerController owner, BagScrapBurstSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        private IEnumerator AnimRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            animTimer = owner.GetCurrentAnimTime();
        }

        public override void OnEnter()
        {
            // 손잡이를 만들고
            Transform createPoint = GameObject.FindWithTag("GrabPoint").transform;
            parent.instance = UnityEngine.Object.Instantiate(parent.tmp, createPoint.position, Quaternion.identity).GetComponent<ScrapParentObject>();
            parent.instance.gameObject.transform.parent = createPoint;
            parent.instance.OnStartEvent.AddListener(OnAction);

            parent.instance.Init(parent.skilldata);

            base.OnEnter();

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

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
            {
                owner.Movement.LookAt(lookDir);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            UnityEngine.Object.Destroy(parent.instance);
        }

        public override void OnAction()
        {
            parent.instance.gameObject.transform.parent = null;
            parent.instance.gameObject.transform.rotation = owner.gameObject.transform.rotation;
            owner.BagSkillHandler.NextStep();
        }
    }

    /// <summary>
    /// 2번 동작 : 고철덩어리 발사하기
    /// </summary>
    public class BagScrapBurst_2 : BaseBagState
    {
        private BagScrapBurstSkill parent;
        private string animName = "ScrapBrust_2";

        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        private float animTimer;

        public BagScrapBurst_2(PlayerController owner, BagScrapBurstSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        private IEnumerator AnimRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            animTimer = owner.GetCurrentAnimTime();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // 게이지 감소
            parent.Use();

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

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (animTimer <= 0)
            {
                owner.BagSkillHandler.NextStep();
            }
            animTimer -= Time.deltaTime;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

}
