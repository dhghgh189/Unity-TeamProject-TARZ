using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BagSkillEnum;

public class BagCompactCanonSkill : BagSkill
{
    /* 특수 정보 */
    public GameObject tmp;                        // 고철 오브젝트 프리팹
    public ScrapMatelObject instance;     // 고철 오브젝트

    public BagCompactCanonSkill(PlayerController owner, BagSkillContainerSO container)
    {
        keyName = BagIndexKey.CompactCanon;
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
        acts.AddLast(new BagCompactCanon_1(owner, this));
        acts.AddLast(new BagCompactCanon_2(owner, this));

        tmp = Resources.Load("Managed/BagSkill/ScrapMetalObject") as GameObject;
    }

    /// <summary>
    /// 1번 동작 : 전방으로 팔을 뻗어 커다란 파편을 생성한다 -> 파편이 점점 커짐
    /// </summary>
    public class BagCompactCanon_1 : BaseBagState
    {
        private BagCompactCanonSkill parent;
        private readonly string animName = "CompactCanon_1";

        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        public BagCompactCanon_1(PlayerController owner, BagCompactCanonSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Transform createPoint = GameObject.FindWithTag("CreatePoint").transform;
            parent.instance = UnityEngine.Object.Instantiate(parent.tmp, createPoint.position, owner.gameObject.transform.rotation).GetComponent<ScrapMatelObject>();
            parent.instance.gameObject.transform.parent = createPoint;

            parent.instance.Init(parent.skilldata);

            owner.Movement.Rigid.velocity = Vector3.zero;

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
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
            {
                owner.Movement.LookAt(lookDir);
            }

            if (parent.instance == null) return;

            if (parent.instance.IsFull)
            {
                parent.instance.gameObject.transform.parent = null;
                owner.BagSkillHandler.NextStep();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            UnityEngine.Object.Destroy(parent.instance.gameObject);
        }

    }

    public class BagCompactCanon_2 : BaseBagState
    {
        private BagCompactCanonSkill parent;
        private readonly string animName = "CompactCanon_2";

        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        private float animTimer;

        public BagCompactCanon_2(PlayerController owner, BagCompactCanonSkill parent) : base(owner)
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
            parent.instance.Throw();
            parent.Use();

            owner.Movement.Rigid.velocity = Vector3.zero;

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
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
            {
                owner.Movement.LookAt(lookDir);
            }

            if (animTimer <= 0)
            {
                owner.BagSkillHandler.NextStep();
            }
            animTimer -= Time.deltaTime;
        }
    }
}
