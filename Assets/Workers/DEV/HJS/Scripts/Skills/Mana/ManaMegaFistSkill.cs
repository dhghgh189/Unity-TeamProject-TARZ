using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마나 3스킬의 필요 데이터
/// </summary>
public enum ManaMegaFistDataType { FistDamage, FistWidth, FistHeight, FistLength, FistSpeed, FistTime, FistAlpha }
/// <summary>
/// 마나 3스킬 : 거대 주먹
/// </summary>
public class ManaMegaFistSkill : IManaSkill
{
    private const string KEY_NAME = "ManaMegaFist";             // 스킬의 고유 이름
    private ManaSkillDataSO skillData;                          // 스킬의 데이터
    public LinkedList<BaseManaState> Acts { get; private set; } // 행동이 들어있는 연결리스트
    public ManaSkillDataSO SkillData { get => skillData; set => skillData = value; }

    public GameObject tmp;  // 차원문 오브젝트를 담아두는 변수

    public ManaMegaFistSkill(PlayerController owner)
    {
        Acts = new LinkedList<BaseManaState>();
        Acts.AddLast(new ManaMegaFist_1(owner, this));

#if UNITY_EDITOR
        tmp = Resources.Load("Unmanaged/Gate") as GameObject;
#else
        tmp = Resources.Load("Managed/ManaSkill/Gate") as GameObject;
#endif
    }

    public void SetInit(ManaSkillHandler manaSkillHandler)
    {
        // 초기 설정
        manaSkillHandler.ActList = Acts;
        skillData = manaSkillHandler.GetData(KEY_NAME);
    }

    /// <summary>
    /// 1번 동작 : 차원문 생성
    /// </summary>
    public class ManaMegaFist_1 : BaseManaState
    {
        private readonly ManaMegaFistSkill parent;
        private string animName = "ManaMegaFist_1";

        private float animTimer;
        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        public ManaMegaFist_1(PlayerController owner, ManaMegaFistSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        public override void OnEnter()
        {
            if (camTrf == null)
                camTrf = Camera.main.transform;

            owner.Movement.Rigid.velocity = Vector3.zero;
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
            if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
            {
                owner.Movement.LookAt(lookDir);
            }
        }

        public override void OnAction()
        {
            Vector3 pos = owner.transform.position + owner.transform.forward * -0.5f;
            // 차원문 생성
            GameObject instatiate = Object.Instantiate(parent.tmp, pos, owner.transform.rotation);
            MegaFistGateObject gate = instatiate.GetComponent<MegaFistGateObject>();
            gate.Init(parent.SkillData);
            owner.ManaSkillHandler.NextStep();
        }
    }

}
