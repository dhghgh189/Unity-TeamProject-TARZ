using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마나 1스킬의 필요 데이터
/// </summary>
public enum ManaRushDataType { RushTime, RushSpeed, GrabDamage, RangeAttackDamage, AttackRange }
/// <summary>
/// 마나 1스킬 : 돌진 잡기
/// </summary>
public class ManaRushSkill : IManaSkill
{
    private const string KEY_NAME = "ManaRush";                 // 스킬의 고유 이름       
    private ManaSkillDataSO skillData;                          // 스킬의 데이터
    public LinkedList<BaseManaState> Acts { get; private set; } // 행동이 들어있는 연결리스트
    public ManaSkillDataSO SkillData { get => skillData; set => skillData = value; }

    public Transform collider;      // 충돌한 오브젝트를 담아두는 변수
    public Quaternion quaternion;   // 해당 충돌체의 방향

    public ManaRushSkill(PlayerController owner)
    {
        Acts = new LinkedList<BaseManaState>();
        Acts.AddLast(new ManaRush_1(owner, this));
        Acts.AddLast(new ManaRush_2(owner, this));
        Acts.AddLast(new ManaRush_3(owner, this));
    }

    public void SetInit(ManaSkillHandler manaSkillHandler)
    {
        // 초기 설정
        manaSkillHandler.ActList = Acts;
        skillData = manaSkillHandler.GetData(KEY_NAME);
    }
}

/// <summary>
/// 1번 동작 : 돌격
/// </summary>
public class ManaRush_1 : BaseManaState
{
    private readonly ManaRushSkill parent;
    private readonly string animName = "ManaRush_1";
    float rushTime;
    float rushSpeed;

    private Transform camTrf;
    private Vector3 moveDir;
    private Vector3 lookDir;

    public ManaRush_1(PlayerController owner, ManaRushSkill parent) : base(owner)
    {
        this.parent = parent;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("마나1 마나 입장");
        rushTime = parent.SkillData.GetData((int)ManaRushDataType.RushTime);
        rushSpeed = parent.SkillData.GetData((int)ManaRushDataType.RushSpeed);


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
        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            owner.Movement.LookAt(lookDir);
        }

        rushTime -= Time.deltaTime;

        if (owner.PInput.TryDash && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        if (rushTime <= 0)
        {
            Debug.Log("마나1 충격파 행동으로 넘어가기 요청!");
            owner.ManaSkillHandler.NextStep(2);
            return;
        }

        owner.Movement.Rigid.velocity = lookDir * rushSpeed;
    }
    public override void OnExit()
    {
        base.OnExit();
        owner.Movement.Rigid.velocity = Vector3.zero;
    }

    public override bool OnCollisionAction(Collision other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            Debug.Log("마나1 잡는 행동으로 넘어가기 요청!");
            parent.collider = other.gameObject.transform;
            parent.quaternion = other.transform.rotation;
            owner.Movement.Rigid.velocity = Vector3.zero;
            return true;
        }

        return false;
    }

}

/// <summary>
/// 2번 동작 : 잡기
/// </summary>
public class ManaRush_2 : BaseManaState
{
    private readonly ManaRushSkill parent;
    private string animName = "ManaRush_2";

    float animTimer;
    private Transform camTrf;
    private Vector3 lookDir;
    float grabDamage;
    Transform grabPoint;

    public ManaRush_2(PlayerController owner, ManaRushSkill parent) : base(owner)
    {
        if (grabPoint == null) grabPoint = GameObject.FindWithTag("GrabPoint").transform;
        this.parent = parent;
        animTimer = 999f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animTimer = 999f;
        grabDamage = parent.SkillData.GetData((int)ManaRushDataType.GrabDamage);

        Debug.Log("마나2 입장");

        if (camTrf == null)
            camTrf = Camera.main.transform;

        // 충돌한 몬스터 손에 잡기
        parent.collider.transform.parent = grabPoint;

        owner.Anim.CrossFade(Animator.StringToHash(animName), 0.01f);
        owner.StartCoroutine(AnimRoutine());
    }
    IEnumerator AnimRoutine()
    {
        // 애니메이션 재생 후 바로 info를 가져오면 이전 클립 정보가 받아지므로
        // 잠시 대기하는 시간을 가져야 한다.
        yield return new WaitForSeconds(0.1f);
        animTimer = owner.GetCurrentAnimTime();
        Debug.Log($"마나2 animTimer : {animTimer}");
    }

    public override void OnUpdate()
    {
        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            owner.Movement.LookAt(lookDir);
        }

        if (animTimer <= 0)
        {
            lookDir = Vector3.zero;
            Debug.Log("마나2 잡기에서 충격파 행동으로 넘어가기 요청!");
            IDamagable damagable = parent.collider.gameObject.GetComponent<IDamagable>();

            if (damagable is not null) damagable.TakeDamage(grabDamage);

            // 잡은 몬스터 놓아주기
            LeaveMonster();

            owner.ManaSkillHandler.NextStep();
        }
        animTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        LeaveMonster();
    }
    private void LeaveMonster()
    {
        parent.collider.transform.parent = null;
        parent.collider.transform.position = owner.transform.position + owner.transform.forward * 0.5f;
        parent.collider.transform.rotation = parent.quaternion;
    }
}

/// <summary>
/// 3번 동작 : 내려꽂기
/// </summary>
public class ManaRush_3 : BaseManaState
{
    private readonly ManaRushSkill parent;
    private string animName = "ManaRush_3";

    float animTimer;
    private Vector3 lookDir;
    float damage;
    float attackRange;
    public ManaRush_3(PlayerController owner, ManaRushSkill parent) : base(owner)
    {
        this.parent = parent;
        animTimer = 999f;
    }

    public override void OnEnter()
    {
        animTimer = 999f;
        Debug.Log("충격파 입장");
        damage = parent.SkillData.GetData((int)ManaRushDataType.RangeAttackDamage);
        attackRange = parent.SkillData.GetData((int)ManaRushDataType.AttackRange);

        owner.Movement.Rigid.velocity = Vector3.zero;

        owner.Anim.CrossFade(Animator.StringToHash(animName), 0.01f);
        owner.StartCoroutine(AnimRoutine());
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
        if (lookDir != Vector3.zero && owner.transform.forward != lookDir)
        {
            owner.Movement.LookAt(lookDir);
        }

        if (owner.PInput.TryDash && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        if (animTimer <= 0)
        {
            lookDir = Vector3.zero;
            Debug.Log("충격파 행동에서 기본으로 돌아가기!");
            owner.ManaSkillHandler.NextStep();
        }

        animTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnAction()
    {
        Debug.Log("범위 공격!");
        Collider[] colliders = Physics.OverlapSphere(owner.transform.position, attackRange, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(damage); Debug.Log($"{collider.gameObject.name}에게 {150}만큼의 피해를 입혔다!"); }
        }
    }

}

