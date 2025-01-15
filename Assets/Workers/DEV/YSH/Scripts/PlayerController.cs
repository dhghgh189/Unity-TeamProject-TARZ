using System;
using System.Collections;
using System.Text;
using UnityEngine;
using Zenject;
using static SkillEnum;

public enum EMachineType { Movement, Attack }

public enum EMpAmountType { Melee, Throw, Length }

public class PlayerController : MonoBehaviour, IDamagable
{
    [Inject] private StatModel stat;
    [HideInInspector][Inject] public Loading loadingObject;
    [HideInInspector][Inject] public InGameSaveData saveData;

    private Animator anim;
    private Coroutine SternCheckRoutine;

    private DrainManager drainManager;
    [SerializeField] private GameObject manaSkillPanel;

    private bool isTryManaSkill;

    public EState currentStateView;
    private TakeHitType currentHitTypeView;

    public Transform cameraLookPos;

    [HideInInspector] public StatusEffect effect;
    [HideInInspector][Inject] public PlayerStateTransfer StateTransfer;
    [HideInInspector] public BagSkillHandler BagSkillHandler;
    [HideInInspector] public ManaSkillHandler ManaSkillHandler;
    [HideInInspector] public PlayerSkillHandler SkillHandler;
    [HideInInspector] public AblityAdapter AblityAdapter;
    [HideInInspector] public Interactioner interactioner;
    public PlayerFSM Fsm { get; private set; }
    public Animator Anim { get { return anim; } }
    public PlayerInputHandler PInput { get; private set; }
    public StatModel Stat { get { return stat; } }
    public PlayerMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }
    public DrainManager Drain { get { return drainManager; } }
    public float delay { get; set; }
    public bool IsAnimStart { get; set; }
    public bool IsGrabingInput { get { return interactioner.IsGrabing; } }

    public bool IsImortal { get; set; }

    public CapsuleCollider coll;

    private StringBuilder sb;

    public Transform GrabPoint { get; private set; }

    void Awake()
    {
        this.BagSkillHandler = GetComponent<BagSkillHandler>();
        drainManager = GetComponentInChildren<DrainManager>();
        this.ManaSkillHandler = GetComponent<ManaSkillHandler>();
        SkillHandler = GetComponent<PlayerSkillHandler>();
        this.AblityAdapter = GetComponent<AblityAdapter>();
        anim = GetComponent<Animator>();
        AblityAdapter = GetComponent<AblityAdapter>();
        PInput = GetComponent<PlayerInputHandler>();
        Movement = GetComponent<PlayerMovement>();
        Attack = GetComponent<PlayerAttack>();
        SkillHandler = GetComponent<PlayerSkillHandler>();
        ManaSkillHandler = GetComponent<ManaSkillHandler>();
        interactioner = GetComponentInChildren<Interactioner>();
        coll = GetComponent<CapsuleCollider>();
        GrabPoint = GameObject.FindWithTag("GrabPoint").transform;
        effect = GetComponentInChildren<StatusEffect>();

        Fsm = new PlayerFSM(this, AblityAdapter, StateTransfer);

        IsAnimStart = false;

        sb = new StringBuilder();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        manaSkillPanel.SetActive(false);
        Debug.Log("다시 시작!");
    }

    private void Update()
    {
        Fsm.OnUpdate();

        SetAnimParam();

        // Mana Skill 처리 구간
        if (Fsm.CurrentState.type == EState.ManaUse
            || Fsm.CurrentState.type == EState.BagUse
            || Fsm.CurrentState.type == EState.Jump
            || Fsm.CurrentState.type == EState.Fall)
        {
            if (isTryManaSkill)
                SetManaSkillState(false);

            return;
        }

        // 마나 스킬 UI 출력 키 입력 감지 
        CheckManaSkillInput();

        // 키 감지 되면 마나스킬 체크
        if (isTryManaSkill)
        {
            for (int i = 0; i < Define.USEKEY_MAXCOUNT; i++)
            {
                if (PInput.UseKeyPressed[i])   // 스킬 1 ~ 4 번 키 입력 감지 
                {
                    TryManaSkill(i);
                    return;
                }
            }
        }
        // 키 감지 안되면 가방스킬 체크
        else
        {
            for (int i = 0; i < Define.USEKEY_MAXCOUNT; i++)
            {
                if (PInput.UseKeyPressed[i])   // 스킬 1 ~ 4 번 키 입력 감지 
                {
                    BagSkillHandler.UseBagSkill(i);
                    return;
                }
            }
        }
    }

    private void CheckManaSkillInput()
    {
        if (PInput.TryManaSkill)
        {
            if (!isTryManaSkill)
            {
                SetManaSkillState(true);
            }
        }
        else
        {
            if (isTryManaSkill) // 무한 반복 방지
            {
                SetManaSkillState(false);
            }
        }
    }

    private void TryManaSkill(int index)
    {
        // 스킬 사용 여부 확인
        bool bSuccess = ManaSkillHandler.UseManaSkill(index);
        if (bSuccess)
        {
            ChangeState(EState.ManaUse);
            SetManaSkillState(false);
        }
    }

    private void SetManaSkillState(bool bOn)
    {
        manaSkillPanel.gameObject.SetActive(bOn);
        isTryManaSkill = bOn;
    }

    private void SetAnimParam()
    {
        anim.SetBool("IsGrounded", Movement.IsGrounded);
    }

    private void FixedUpdate()
    {
        Fsm.OnFixedUpdate();
        return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + transform.up * 0.06f, new Vector3(0.5f, 0.1f, 0.5f));
    }

    public void AddObjectStack(ThrowObject tobj)
    {
        Attack.AddObjectStack(tobj);
    }

    public void ChangeState(EState state)
    {
        Fsm.ChangeState(state);
    }

    public BaseState<PlayerController> GetCurrentState()
    {
        return Fsm.CurrentState;
    }

    public void TakeDamage(float damage)
    {
        if (stat.CurrentHp <= 0) return;

        if (IsImortal)
        {
            Debug.Log("무적 판정!!");
            return;
        }

        if (CheatManager.isMujeok)
        {
            Debug.Log("무적이당");
            return;
        }

        // 점프 근접 공격 중 피격당하면 종료시키기
        if (!Attack.IsEndJumpMelee)
        {
            Attack.EndJumpMelee();
        }

        Debug.Log("아야");
        Stat.CurrentHp -= damage;
        if (Stat.CurrentHp > 0)
        {
            if (SternCheckRoutine != null) return;

            // 사운드 재생
            // 데미지가 최대 체력의 50% 이상인 경우 개별 사운드 재생
            if (damage >= (Stat.MaxHp * 0.5f))
            {
                SoundManager.PlaySFX(SoundManager.SoundData_P.TakeDamage_Danger);
            }
            else
            {
                SoundManager.PlaySFX(SoundManager.SoundData_P.TakeDamage_Normal);
            }
            anim.CrossFade(Define.HASH_ANIM_DAMAGED, 0.1f);
            delay = GetCurrentAnimTime() * 0.5f;
            IsAnimStart = true;

            SternCheckRoutine = StartCoroutine(SternRoutine(delay));
        }
        else
        {
            ChangeState(EState.Dead);
        }

        /* 추후 합의 후 재진행 예정
        switch (currentHitTypeView)
        {
            case TakeHitType.Stern:
                {
                    Debug.Log("스턴됨!");
                    break;
                }
            case TakeHitType.Knock_Back:
                {
                    Debug.Log("넉백됨!");
                    break;
                }
            default:
                {
                    // 임시
                    currentHitTypeView = TakeHitType.Size;
                    break;
                }
        }*/
    }

    IEnumerator SternRoutine(float cool)
    {
        float MaxCool = cool;

        while (!IsAnimStart)
        {
            yield return null;
        }
        while (cool > 0.1f)
        {
            cool -= Time.deltaTime;
            Movement.Rigid.angularVelocity = Vector3.zero;
            Movement.Rigid.velocity = new Vector3(0, Movement.Rigid.velocity.y, 0);
            yield return null;
        }

        IsAnimStart = false;
        SternCheckRoutine = null;
        yield break;
    }

    public bool IsEnoughStamina(float amount)
    {
        Debug.Log($"<color=cyan>Current Stamina : {stat.CurrentStamina}, Amount : {amount}</color>");
        if (stat.CurrentStamina == 0)
        {
            SoundManager.PlaySFX(SoundManager.SoundData_P.Alert);
        }
        //return stat.CurrentStamina >= amount;
        // 조건 변경 (스테미너가 1이라도 존재한다면 사용 가능)
        return stat.CurrentStamina > 0;
    }

    public float GetCurrentAnimTime(int layer = 0)
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(layer);
        // 현재 재생되는 애니메이션의 총 길이와 speed를 계산하여 실제 재생 시간을 반환 
        return (info.length / info.speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        sb.Clear();
        sb.Append(currentStateView);

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")) && Enum.IsDefined(typeof(ActTimingType), sb.ToString()))
        {
            //SkillHandler.PlayerCollision((ActTimingType)Enum.Parse(typeof(ActTimingType), sb.ToString()), collision.gameObject);
        }
        SkillHandler.CollisionEvent(currentStateView, collision.gameObject);
    }

    public void InfStamina(float value)
    {

        steminaRoutine = StartCoroutine(SteminaRoutine(value));

    }

    Coroutine steminaRoutine;
    IEnumerator SteminaRoutine(float value)
    {
        Debug.Log("루틴 시작합니다~!~!");
        Stat.StaminaCostRate = 0;
        Stat.CurrentStamina = Stat.MaxStamina;
        yield return Util.GetDelay(value);
        Stat.StaminaCostRate = 1;

        steminaRoutine = null;
        Debug.Log("종료로그");
    }

    private void OnDisable()
    {
        if (SternCheckRoutine != null)
        {
            StopCoroutine(SternCheckRoutine);
            SternCheckRoutine = null;
        }
    }
}
