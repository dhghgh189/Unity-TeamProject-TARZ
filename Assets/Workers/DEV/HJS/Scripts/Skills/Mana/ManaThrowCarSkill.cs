using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 마나 2스킬의 필요 데이터
/// </summary>
public enum ManaThrowCarDataType { FlightSpeed, HitDamage, ExplosionDamage, ExplosionRange }
/// <summary>
/// 마나 2스킬 : 차량 투척
/// </summary>
public class ManaThrowCarSkill : IManaSkill
{
    private const string KEY_NAME = "ManaThrowCar";             // 스킬의 고유 이름
    private ManaSkillDataSO skillData;                          // 스킬의 데이터
    public LinkedList<BaseManaState> Acts { get; private set; } // 행동이 들어있는 연결리스트
    public ManaSkillDataSO SkillData { get => skillData; set => skillData = value; }

    public GameObject tmp;          // 차 오브젝트가 담겨있는 변수
    public GameObject carInstance;  // 차  오브젝트로 Instatiate했을 때 담는 변수
    public UnityEvent OnThrowEvent { get; private set; }    // 차 던지는 애니메이션을 실행시켜줄 이벤트

    public ManaThrowCarSkill(PlayerController owner)
    {
        Acts = new LinkedList<BaseManaState>();
        Acts.AddLast(new ManaThrowCar_1(owner, this));
        Acts.AddLast(new ManaThrowCar_2(owner, this));
        OnThrowEvent = new UnityEvent();
        tmp = Resources.Load("Unmanaged/Car") as GameObject;
    }

    public void SetInit(ManaSkillHandler manaSkillHandler)
    {
        manaSkillHandler.ActList = Acts;
        skillData = manaSkillHandler.GetData(KEY_NAME);
    }

    /// <summary>
    /// 1번 동작 : 차량 집어들기
    /// </summary>
    public class ManaThrowCar_1 : BaseManaState
    {
        private readonly ManaThrowCarSkill parent;
        private string animName = "ManaThrowCar_1";

        private float animTimer;
        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        public ManaThrowCar_1(PlayerController owner, ManaThrowCarSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        public override void OnEnter()
        {
            parent.carInstance = null;
            animTimer = 999f;
            Debug.Log("차량 집어들기 시작!");

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


            // 손을 드는 애니메이션 실행
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
            if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
            {
                owner.Movement.LookAt(lookDir);
            }

            // 차량 생성을 완료 했다면 다음 스탭
            if (animTimer <= 0f)
            {
                Debug.Log("차량 집어들기 에서 차량 던지기로 요청!");
                owner.ManaSkillHandler.NextStep();
            }

            animTimer -= Time.deltaTime;
        }

        public override void OnAction()
        {
            parent.carInstance = Object.Instantiate(parent.tmp, owner.gameObject.transform.position + Vector3.up * 3.5f, owner.gameObject.transform.rotation * Quaternion.Euler(Vector3.right * 45));
            ThrowCarObject throwCarObject = parent.carInstance.GetComponent<ThrowCarObject>();
            if (throwCarObject is not null)
            {
                throwCarObject.Init(parent.SkillData);
                parent.OnThrowEvent.AddListener(throwCarObject.Throw);
            }
        }

        public override void OnExit()
        {
            if (parent.carInstance is not null)
            {
                Object.Destroy(parent.carInstance);
                parent.carInstance = null;
            }
        }
    }
    /// <summary>
    /// 2번 동작 : 차량 투척
    /// </summary>
    public class ManaThrowCar_2 : BaseManaState
    {
        private readonly ManaThrowCarSkill parent;
        private string animName = "ManaThrowCar_2";

        private float animTimer;

        public ManaThrowCar_2(PlayerController owner, ManaThrowCarSkill parent) : base(owner)
        {
            this.parent = parent;
        }

        public override void OnEnter()
        {
            Debug.Log("차량 던지기 시작");
            animTimer = 999f;

            // 차량 투척 애니메이션 실행
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
            if (animTimer <= 0)
            {
                Debug.Log("차량 던지기 종료!");
                owner.ManaSkillHandler.NextStep();
            }

            animTimer -= Time.deltaTime;
        }

        public override void OnAction()
        {
            Debug.Log("차량 던졌다!");
            parent.OnThrowEvent?.Invoke();
            parent.carInstance = null;

        }
        public override void OnExit()
        {
            parent.OnThrowEvent.RemoveAllListeners();
            if (parent.carInstance is not null)
            {
                Object.Destroy(parent.carInstance);
                parent.carInstance = null;
            }
        }
    }
}

