using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public enum ManaBlackHollDataType { ExplosionMinDamage, ExplosionMaxDamage, AbsorptionMinRange, AbsorptionMaxRange, AbsorptionSpeed, ThrowSpeed, FlightTime, ExplosionRange }
public class ManaBlackHollSkill : IManaSkill
{
    private const string KEY_NAME = "ManaBlackHoll_1";
    private ManaSkillDataSO skillData;
    public LinkedList<BaseManaState> Acts { get; private set; }
    public ManaSkillDataSO SkillData { get => skillData; set => skillData = value; }

    public GameObject tmp;
    public GameObject blackhollInstance;
    public UnityEvent OnThrowEvent { get; private set; }

    public ManaBlackHollSkill(PlayerController owner)
    {
        Acts = new LinkedList<BaseManaState>();
        OnThrowEvent = new UnityEvent();
        tmp = Resources.Load("Unmanaged/BlackHoll") as GameObject;
    }

    public void SetInit(ManaSkillHandler manaSkillHandler)
    {
        manaSkillHandler.ActList = Acts;
        skillData = manaSkillHandler.GetData(KEY_NAME);
    }

    /// <summary>
    /// 1번 동작 : 블랙홀 생성
    /// </summary>
    public class ManaBlackHoll_1 : BaseManaState
    {
        private readonly ManaBlackHollSkill parent;
        private string animName = "ManaBlackHoll_1";

        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        private Transform createPoint;
        private BlackHollObject blackHoll;

        public ManaBlackHoll_1(PlayerController owner, ManaBlackHollSkill parent) : base(owner)
        {
            this.parent = parent;
            if(createPoint == null) createPoint = GameObject.FindWithTag("CreatePoint").transform;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("블랙홀 생성 시작");

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
            // 시야를 고정하고
            // 무슨 버튼? 누르면 즉시 나가는거 같다
            // 그 예외처리 실행

            // else 로 한 이유 -> 누른 타이밍이 애니메이션이 끝나는 타이밍이면 동시에 되서, 입력이 우선
            if(blackHoll.CanThrow && owner.PInput.TryThrow)
            {
                Debug.Log("블랙홀을 지금 발포하기 요청!");
                owner.ManaSkillHandler.NextStep();
            }
            else if (blackHoll.FullCharge)
            {
                Debug.Log("블랙홀을 최대치 만큼 생성 후 발포 요청!");
                owner.ManaSkillHandler.NextStep();
            }

        }

        public override void OnAction()
        {
            // 생성하는 부분
            parent.blackhollInstance = Object.Instantiate(parent.tmp, createPoint.position, createPoint.rotation);
            
            blackHoll = parent.blackhollInstance.GetComponent<BlackHollObject>();
            if (blackHoll != null) 
            { 
                // 이벤트 연결
                parent.OnThrowEvent.AddListener(blackHoll.Throw);
            }
            else
            {
                Debug.Log("블랙홀 던지기 연결 안됨!");
            }
        }

        public override void OnExit()
        {
            parent.OnThrowEvent.RemoveAllListeners();
            if (parent.blackhollInstance is not null)
            {
                Object.Destroy(parent.blackhollInstance);
                parent.blackhollInstance = null;
            }
        }
    }

    /// <summary>
    /// 2번 동작 : 블랙홀 발사
    /// </summary>
    public class ManaBlackHoll_2 : BaseManaState
    {
        private readonly ManaBlackHollSkill parent;
        private string animName = "ManaBlackHoll_2";

        private float animTimer;
        private Transform camTrf;
        private Vector3 moveDir;
        private Vector3 lookDir;

        public ManaBlackHoll_2(PlayerController owner, ManaBlackHollSkill parent) : base(owner)
        {
            this.parent = parent;
        }


        public override void OnEnter()
        {
            base.OnEnter();

            Debug.Log("블랙홀 발사 시작");

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

            parent.OnThrowEvent?.Invoke();

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
                Debug.Log("차량 발사 종료!");
                owner.ManaSkillHandler.NextStep();
            }

            animTimer -= Time.deltaTime;

            // 애니메이션 길이만큼 뒤로 가기(반동)
            owner.Movement.Rigid.velocity = -lookDir * Time.deltaTime;
        }

        public override void OnExit()
        {
            parent.OnThrowEvent.RemoveAllListeners();
        }
    }
}
