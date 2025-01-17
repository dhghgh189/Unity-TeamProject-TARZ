using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MonsterData;

/// <summary>
/// 마나 1스킬의 필요 데이터
/// </summary>
public enum ManaRushDataType { RushTime, RushSpeed, GrabDamage, RangeAttackDamage, AttackRange, CreateThrowObject }
/// <summary>
/// 마나 1스킬 : 돌진 잡기
/// </summary>
public class ManaRushSkill : IManaSkill
{
    private const string KEY_NAME = "ManaRush";                 // 스킬의 고유 이름       
    private ManaSkillDataSO skillData;                          // 스킬의 데이터
    public LinkedList<BaseManaState> Acts { get; private set; } // 행동이 들어있는 연결리스트
    public ManaSkillDataSO SkillData { get => skillData; set => skillData = value; }

    public Collider collider;      // 충돌한 오브젝트를 담아두는 변수
    public Quaternion quaternion;   // 해당 충돌체의 방향
    public Vector3 pos;             // 해당 충돌체의 기존 위치

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
        parent.collider = null;
    }

    public override bool OnCollisionAction(Collision other)
    {
        if (parent.collider != null) return false;

        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            // 일반 몬스터일 때
            if (other.gameObject.GetComponent<MonsterData>().MonsterTIer.Equals(MonsterTier.Normal))
            {
                Debug.Log("마나1 잡는 행동으로 넘어가기 요청!");
                // 잡힌 적의 정보 저장
                parent.collider = other.collider;
                parent.pos = other.gameObject.transform.position;
                parent.quaternion = other.transform.rotation;
                // 충돌 끄기
                Physics.IgnoreCollision(owner.coll, other.collider, true);

                parent.collider.GetComponent<MonsterData>().IsCatched = true;
                parent.collider.GetComponent<NavMeshAgent>().enabled = false;
                parent.collider.GetComponent<Rigidbody>().useGravity = false;
                parent.collider.GetComponent<Rigidbody>().isKinematic = true;

                owner.Movement.Rigid.velocity = Vector3.zero;
                return true;
            }
            // 보스나 엘리트일 때
            else
            {
                owner.ManaSkillHandler.NextStep(2);
                return false;
            }
        }
        else if(
            ((1 << other.gameObject.layer) & LayerMask.GetMask("Camera")) != 0 ||
            ((1 << other.gameObject.layer) & LayerMask.GetMask("Obstacles")) != 0)
        {
            owner.ManaSkillHandler.NextStep(2);
            return false;
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
        parent.collider.transform.localPosition = Vector3.zero;

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

            if (damagable is not null) damagable.TakeDamage(grabDamage * owner.Stat.SkillPowerPer);

            // 잡은 몬스터 놓아주기
            LeaveMonster();

            owner.ManaSkillHandler.NextStep();
        }
        animTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        LeaveMonster();
        parent.collider.GetComponent<MonsterData>().IsCatched = false;
        parent.collider.GetComponent<NavMeshAgent>().enabled = true;
        parent.collider.GetComponent<Rigidbody>().useGravity = false;
        parent.collider.GetComponent<Rigidbody>().isKinematic = true;
        Physics.IgnoreCollision(owner.coll, parent.collider, false);
        parent.collider = null;
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

    /* 파편 생성 변수 */
    private Vector3 dir;
    private Vector3 pos;
    private float count;
    private List<GameObject> throwObjectList;

    public ManaRush_3(PlayerController owner, ManaRushSkill parent) : base(owner)
    {
        this.parent = parent;
        animTimer = 999f;
        throwObjectList = new();
    }

    public override void OnEnter()
    {
        if (parent.collider is not null)
        {
            Physics.IgnoreCollision(owner.coll, parent.collider, false);
            parent.collider.GetComponent<MonsterData>().IsCatched = false;
            parent.collider.GetComponent<NavMeshAgent>().enabled = true;
            parent.collider.GetComponent<Rigidbody>().useGravity = false;
            parent.collider.GetComponent<Rigidbody>().isKinematic = true;
        }

        animTimer = 999f;
        Debug.Log("충격파 입장");
        damage = parent.SkillData.GetData((int)ManaRushDataType.RangeAttackDamage);
        attackRange = parent.SkillData.GetData((int)ManaRushDataType.AttackRange);

        owner.Movement.Rigid.velocity = Vector3.zero;

        Debug.Log("범위 공격!");
        // 이펙트 생성
        EffectManager.instance.ParticlePlay("ManaSkill_11", 1f, owner.transform.position, Quaternion.identity);
        // SFX 재생
        SoundManager.PlaySFX(SoundManager.SoundData_S.ManaSkillSounds_1[0].AudioClip);
        // 던지는 물건 생성
        CreateThrowObject();
        
        Collider[] colliders = Physics.OverlapSphere(owner.transform.position, attackRange, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(damage * owner.Stat.SkillPowerPer); }
        }

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
        parent.collider = null;
    }

    private void CreateThrowObject()
    {
        // 던지는 물체의 갯수
        float count = parent.SkillData.GetData((int)ManaRushDataType.CreateThrowObject);
        float radius = 2f;
        Collider[] check = new Collider[1];

        dir = Vector3.zero;
        pos = owner.gameObject.transform.position;

        // 던지는 물체 원형으로 생성
        for (int i = 0; i < count; i++)
        {
            float angle = i * (Mathf.PI * 2.0f) / count;

            GameObject child = Object.Instantiate(owner.ManaSkillHandler.instance, pos, Quaternion.identity).gameObject;
            throwObjectList.Add(child);

            child.transform.position
                = pos + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))) * radius + Vector3.up;

            dir = child.transform.position - pos;
            child.transform.rotation = Quaternion.LookRotation(dir.normalized);
        }

        // Addforce로 날리기
        foreach (GameObject item in throwObjectList)
        {
            Rigidbody rigid = item.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                item.GetComponent<Rigidbody>().AddForce((item.transform.forward) * 5f, ForceMode.Impulse);
            }
        }
    }
}

