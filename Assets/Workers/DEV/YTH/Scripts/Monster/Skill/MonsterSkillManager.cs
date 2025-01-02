using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSkillManager : MonoBehaviour
{
    #region Skill-ScriptableObj

    const string SKILL_PATH = "Managed/Skill/Monster";

    [Header("MonsterSkill")]
    private MonsterSkill _bomb;
    public MonsterSkill BombSkill { get { return _bomb; } set { _bomb = value; } }

    private MonsterSkill _dashAttack;
    public MonsterSkill DashAttackSkill { get { return _dashAttack; } set { _dashAttack = value; } }

    private MonsterSkill _jumpAttack;
    public MonsterSkill JumpAttackSkill { get { return _jumpAttack; } set { _jumpAttack = value; } }

    private MonsterSkill _mine;
    public MonsterSkill MineSkill { get { return _mine; } set { _mine = value; } }

    private MonsterSkill _stimPak;
    public MonsterSkill StimPakSkill { get { return _stimPak; } set { _stimPak = value; } }

    private MonsterSkill _wheelWind;
    public MonsterSkill WheelWindSkill { get { return _wheelWind; } set { _wheelWind = value; } }

    private MonsterSkill _electricWall;
    public MonsterSkill ElectricWallSkill { get { return _electricWall; } set { _electricWall = value; } }

    private MonsterSkill _thunder;
    public MonsterSkill ThunderSkill { get { return _thunder; } set { _thunder = value; } }

    private MonsterSkill _trippleAttack;
    public MonsterSkill TrippleAttackSkill { get { return _trippleAttack; } set { _trippleAttack = value; } }

    private MonsterSkill _frogJumpAttack;
    public MonsterSkill FrogJumpAttackSkill { get { return _frogJumpAttack; } set { _frogJumpAttack = value; } }

    private MonsterSkill _revive;
    public MonsterSkill ReviveSkill { get { return _revive; } set { _revive = value; } }
    #endregion

    #region Prefab
    [Header("Prefab")]

    [Header("Range")]
    [SerializeField] GameObject _projectile;

    [Header("Bomber")]
    [SerializeField] GameObject _bombPrefab;

    [SerializeField] GameObject _minePrefab;

    [SerializeField] GameObject _jackTheRipper;

    [Header("Arnold")]
    [SerializeField] GameObject _electricWallPrefab;

    [SerializeField] GameObject _thunderPrefab;


    private GameObject _reviveBefore; // 불러올거에요 비워놔주세요
    public GameObject ReviveBefore { get { return _reviveBefore; } set { _reviveBefore = value; } }

    private GameObject _reviveAfter; // 불러올거에요 비워놔주세요
    public GameObject ReviveAfter { get { return _reviveAfter;  } set { _reviveAfter = value; } } 
    #endregion

    #region Etc
    [Header("Etc")]
    private Animator _animator;

    private Transform _muzzlePoint; // 불러올거에요 비워놔주세요
    public Transform MuzzlePoint { get { return _muzzlePoint; } set { _muzzlePoint = value; } }

    private GameObject _wheelWindTrigger; // 불러올거에요 비워놔주세요
    public GameObject WheelWindTrigger { get { return _wheelWindTrigger; } set { _wheelWindTrigger = value; } }

    private Vector3 _electricWallPosition;

    private Vector3 _electricWallPosition2;
  
    private Vector3 _jumpStartPosition;

    private Vector3 _jumpDirection;

    private float _elapsedTime = 0;

    private MonsterData _monsterData;

    private LayerMask WhatIsTarget;

    private PooledObject _pooledObject;

    private PlayerController _player;
    #endregion

    private void Awake()
    {
        _monsterData = GetComponent<MonsterData>();
        WhatIsTarget = (1 << LayerMask.NameToLayer("Player"));
        _pooledObject = GetComponent<PooledObject>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        LoadSkill();
        SkillInit();

        _player = _pooledObject.player;
    }

    public void LoadSkill()
    {
        _bomb = Resources.Load<MonsterSkill>($"{SKILL_PATH}/Bomb");
        _dashAttack = Resources.Load<MonsterSkill>($"{SKILL_PATH}/DashAttack");
        _jumpAttack = Resources.Load<MonsterSkill>($"{SKILL_PATH}/JumpAttack");
        _mine = Resources.Load<MonsterSkill>($"{SKILL_PATH}/Mine");
        _stimPak = Resources.Load<MonsterSkill>($"{SKILL_PATH}/StimPak");
        _wheelWind = Resources.Load<MonsterSkill>($"{SKILL_PATH}/WheelWind");
        _electricWall = Resources.Load<MonsterSkill>($"{SKILL_PATH}/ElectricWall");
        _thunder = Resources.Load<MonsterSkill>($"{SKILL_PATH}/Thunder");
        _trippleAttack = Resources.Load<MonsterSkill>($"{SKILL_PATH}/TrippleAttack");
        _frogJumpAttack = Resources.Load<MonsterSkill>($"{SKILL_PATH}/FrogJumpAttack");
        _revive = Resources.Load<MonsterSkill>($"{SKILL_PATH}/Revive");
    }

    public void SkillInit()
    {
        // 아놀드 스킬 초기화
        JumpAttackSkill.CanUseSkill = true;
        DashAttackSkill.CanUseSkill = true;
        ElectricWallSkill.CanUseSkill = true;
        ThunderSkill.CanUseSkill = true;

        // 폭탄 좀비 스킬 초기화
        MineSkill.CanUseSkill = true;
        BombSkill.CanUseSkill = true;
        StimPakSkill.CanUseSkill = true;

        // 잭더리퍼 스킬 초기화
        WheelWindSkill.CanUseSkill = true;
        TrippleAttackSkill.CanUseSkill = true;

        // 부활 좀비 스킬 초기화
        ReviveSkill.CanUseSkill = true;
    }

    #region JumpAttack
    public Coroutine jumpAttackRoutine;
    public IEnumerator JumpAttackRoutine() // 보스의 도약해서 착지하여 범위 공격
    {
        _jumpAttack.CanUseSkill = false;
        /*_animator.SetTrigger("JumpAttack");*/

        if (jumpRoutine_jumpAttack == null)
        {
            yield return Util.GetDelay(0.8f);
            jumpRoutine_jumpAttack = StartCoroutine(JumpRoutine_JumpAttack());
            Debug.Log("점프!!");
        }

        yield return Util.GetDelay(_jumpAttack.CoolTime);
        jumpAttackRoutine = null;
        _jumpAttack.CanUseSkill = true;
    }

    #region 데미지
    private void JumpAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _jumpAttack.Range);
        foreach (Collider collider in colliders)
        {
            // 공격 범위 확인
            Vector3 source = transform.position;
            source.y = 0;
            Vector3 destination = collider.transform.position;
            destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            if (targetAngle > _jumpAttack.Angle) // 앵글의 반절만
                continue;

            IDamagable damageble = collider.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.TakeDamage(_jumpAttack.Damage);
            }
        }
    }
    #endregion

    #region JumpAttack - 점프 코루틴
    Coroutine jumpRoutine_jumpAttack;
    IEnumerator JumpRoutine_JumpAttack()
    {
        _jumpStartPosition = transform.position;
        _jumpDirection = transform.forward.normalized * JumpAttackSkill.JumpDistance;

        while (_elapsedTime < JumpAttackSkill.InAirTime)
        {
            float yOffset = Mathf.Sin((_elapsedTime / JumpAttackSkill.InAirTime) * Mathf.PI) * JumpAttackSkill.JumpHeight;
            Vector3 zOffset = _jumpDirection * (_elapsedTime / JumpAttackSkill.InAirTime);

            transform.position = _jumpStartPosition + zOffset + new Vector3(0, yOffset, 0);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = _jumpStartPosition + _jumpDirection;
        jumpRoutine_jumpAttack = null;
        _elapsedTime = 0;
    }
    #endregion

    #endregion

    #region WheelWind
    public Coroutine wheelWindRoutine;
    public IEnumerator WheelWindRoutine() // 가렌 E
    {
        WheelWindSkill.CanUseSkill = false;

        _wheelWindTrigger.SetActive(true);

        Radiation jackRadiation = _wheelWindTrigger.GetComponentInChildren<Radiation>();
        jackRadiation.Interaval = WheelWindSkill.Interval;
        jackRadiation.Damage = WheelWindSkill.Damage;

        yield return Util.GetDelay(WheelWindSkill.Duration);
        _wheelWindTrigger.SetActive(false);

        yield return Util.GetDelay(WheelWindSkill.CoolTime);
        wheelWindRoutine = null;
        WheelWindSkill.CanUseSkill = true;
    }
    #endregion

    #region Bomb
    public Coroutine bombRoutine;
    public IEnumerator BombRoutine()  // 직스 궁 
    {
        /*_animator.SetTrigger("");*/

        BombSkill.CanUseSkill = false;

        GameObject bomb = Instantiate(_bombPrefab, _muzzlePoint.position, _muzzlePoint.rotation);

         bomb.GetComponent<Projectile_Bomb>()._bombZombie = gameObject;

        Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
        bombRb.AddForce((_muzzlePoint.forward + _muzzlePoint.up * 3) * BombSkill.ThrowForce, ForceMode.Impulse);
        yield return Util.GetDelay(BombSkill.CoolTime);    
        bombRoutine = null;
        BombSkill.CanUseSkill = true;

    }
    #endregion

    #region Mine
    public Coroutine mineRoutine;
    public IEnumerator MineRoutine()
    {
        /*_animator.SetTrigger("");*/
        MineSkill.CanUseSkill = false;

        GameObject mine = Instantiate(_minePrefab, _muzzlePoint.position, _muzzlePoint.rotation);
        mine.GetComponent<Projectile_Mine>()._bombZombie = gameObject;
        Rigidbody mineRb = mine.GetComponent<Rigidbody>();
        mineRb.AddForce(_muzzlePoint.forward * MineSkill.ThrowForce, ForceMode.Impulse);

        yield return Util.GetDelay(MineSkill.CoolTime);
        mineRoutine = null;

        MineSkill.CanUseSkill = true;
    }
    #endregion

    #region StimPak
    public void StimPak() // 폭탄좀비가 잭더리퍼의 몬스터 데이터에 접근해서 스텟 업 해줌
    {
        /*_animator.SetTrigger("");*/

        StimPakSkill.CanUseSkill = false;

        if (_jackTheRipper == null)
            return;

        MonsterData JackData = _jackTheRipper.GetComponent<MonsterData>();
        JackData.CurHp += 50;

        StimPakSkill.CanUseSkill = true;
    }
    #endregion

    #region DashAttack
    public Coroutine dashAttackRoutine;
    public IEnumerator DashAttackRoutine()
    {
        DashAttackSkill.CanUseSkill = false;
        _animator.SetBool("DashAttack", true);

        if (jumpRoutine_dashAttack == null)
        {
            jumpRoutine_dashAttack = StartCoroutine(JumpRoutine_dashAttack());
            Debug.Log("점프!!");
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _dashAttack.Range, WhatIsTarget);
        foreach (Collider collider in colliders)
        {
            // 공격 범위 확인
            Vector3 source = transform.position;
            source.y = 0;
            Vector3 destination = collider.transform.position;
            destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            if (targetAngle > _dashAttack.Angle)
                continue;

            IDamagable damageble = collider.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.TakeDamage(_dashAttack.Damage);
            }
        }

        yield return Util.GetDelay(1f);
        _animator.SetBool("DashAttack", false);

        yield return Util.GetDelay(DashAttackSkill.CoolTime);
        dashAttackRoutine = null;
        DashAttackSkill.CanUseSkill = true;
    }

    #region DashAttack - 점프 코루틴
    Coroutine jumpRoutine_dashAttack;
    IEnumerator JumpRoutine_dashAttack()
    {
        _jumpStartPosition = transform.position;
        _jumpDirection = transform.forward.normalized * DashAttackSkill.JumpDistance;

        while (_elapsedTime < DashAttackSkill.InAirTime)
        {
            float yOffset = Mathf.Sin((_elapsedTime / DashAttackSkill.InAirTime) * Mathf.PI) * DashAttackSkill.JumpHeight;
            Vector3 zOffset = _jumpDirection * (_elapsedTime / DashAttackSkill.InAirTime);

            transform.position = _jumpStartPosition + zOffset + new Vector3(0, yOffset, 0);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = _jumpStartPosition + _jumpDirection;
        jumpRoutine_dashAttack = null;
        _elapsedTime = 0;
    }
    #endregion

    #endregion

    #region ElectricWall
    public Coroutine electricWallRoutine;
    public IEnumerator ElectricWallRoutine()
    {
        ElectricWallSkill.CanUseSkill = false;
/*        _animator.SetTrigger("ElectricWall");
*/      

        _electricWallPosition = transform.position + transform.forward * 5f;

        GameObject electricWall = Instantiate(_electricWallPrefab, _electricWallPosition, transform.rotation);

        for (int i = 0; i < 6; i++)
        {
            yield return Util.GetDelay(ElectricWallSkill.Interval);
            _electricWallPosition2 = electricWall.transform.position + electricWall.transform.forward * (7f * (i + 1));
            GameObject electricWall2 = Instantiate(_electricWallPrefab, _electricWallPosition2, electricWall.transform.rotation);
        }

        yield return Util.GetDelay(2.5f);

        yield return Util.GetDelay(ElectricWallSkill.CoolTime);
        electricWallRoutine = null;
        ElectricWallSkill.CanUseSkill = true;
    }
    #endregion

    #region 낙뢰
    public Coroutine thunderRoutine;
    public IEnumerator ThunderRoutine()
    {
        ThunderSkill.CanUseSkill = false;
/*        _animator.SetTrigger("Thunder");
*/
        for (int i = 0; i < 11; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
            Instantiate(_thunderPrefab, randomPos, Quaternion.identity);
            yield return Util.GetDelay(0.1f);
            Vector3 randomPos2 = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
            Instantiate(_thunderPrefab, randomPos2, Quaternion.identity);
            i++;
            yield return Util.GetDelay(1f);
        }

        yield return Util.GetDelay(ThunderSkill.CoolTime);
        thunderRoutine = null;
        ThunderSkill.CanUseSkill = true;
    }
    #endregion

    #region TrippleAttack
    public Coroutine trippleAttackRoutine;
    public IEnumerator TrippleAttackRoutine() 
    {
        TrippleAttackSkill.CanUseSkill = false;

        yield return Util.GetDelay(TrippleAttackSkill.CoolTime);
        trippleAttackRoutine = null;
        TrippleAttackSkill.CanUseSkill = true;
    }

    #region 데미지
    private void TrippleAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, TrippleAttackSkill.Range);
        foreach (Collider collider in colliders)
        {
            // 공격 범위 확인
            Vector3 source = transform.position;
            source.y = 0;
            Vector3 destination = collider.transform.position;
            destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            if (targetAngle > TrippleAttackSkill.Angle * 0.5f) // 앵글의 반절만
                continue;

            IDamagable damageble = collider.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.TakeDamage(TrippleAttackSkill.Damage);
            }
        }
    }
    #endregion

    #endregion

    #region 일반 공격 (범위 설정 가능)
    public void Attack()
    {
        float _range = 0;
        float _angle = 0;
        float _damage = 0;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _range , WhatIsTarget);
        foreach (Collider collider in colliders)
        {
            // 공격 범위 확인
            Vector3 source = transform.position;
            source.y = 0;
            Vector3 destination = collider.transform.position;
            destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            if (targetAngle > _angle * 0.5f) // 앵글의 반절만
                continue;

            IDamagable damageble = collider.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.TakeDamage(_damage);
            }
        }
    }
    #endregion

    #region Jump 로직 코루틴
    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()
    {
        float InAirTime = 0; // 체공 시간
        float JumpHeight = 0; // Y축 점프 높이
        float JumpDistance = 0; // Z축 점프 거리

        _jumpStartPosition = transform.position;
        _jumpDirection = transform.forward.normalized * JumpDistance;

        while (_elapsedTime < InAirTime)
        {
            float yOffset = Mathf.Sin((_elapsedTime / InAirTime) * Mathf.PI) * JumpHeight;
            Vector3 zOffset = _jumpDirection * (_elapsedTime / InAirTime);

            transform.position = _jumpStartPosition + zOffset + new Vector3(0, yOffset, 0);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = _jumpStartPosition + _jumpDirection;
        jumpRoutine_jumpAttack = null;
        _elapsedTime = 0;
    }
    #endregion

    #region FrogJumpAttack
    public Coroutine frogJumpAttackRoutine;
    public IEnumerator FrogJumpAttackRoutine()
    {
        _animator.SetTrigger("JumpAttack");

        if (jumpRoutine_frogJumpAttack == null)
        {
            jumpRoutine_frogJumpAttack = StartCoroutine(JumpRoutine_frogJumpAttack());
            Debug.Log("점프!!");
        }
        yield return null;
        frogJumpAttackRoutine = null;
    }

    #region FrogJumpAttack - 점프 코루틴
    Coroutine jumpRoutine_frogJumpAttack;
    IEnumerator JumpRoutine_frogJumpAttack()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        _jumpStartPosition = transform.position;
        _jumpDirection = transform.forward.normalized * distance;

        while (_elapsedTime < FrogJumpAttackSkill.InAirTime)
        {
            float yOffset = Mathf.Sin((_elapsedTime / FrogJumpAttackSkill.InAirTime) * Mathf.PI) * FrogJumpAttackSkill.JumpHeight;
            Vector3 zOffset = _jumpDirection * (_elapsedTime / FrogJumpAttackSkill.InAirTime);

            transform.position = _jumpStartPosition + zOffset + new Vector3(0, yOffset, 0);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = _jumpStartPosition + _jumpDirection;
        jumpRoutine_frogJumpAttack = null;
        _elapsedTime = 0;
    }
    #endregion

    #endregion

    #region Revive
    public void Revive()
    {
        ReviveSkill.CanUseSkill = false;
        //_animator.SetTrigger("Revive");

        _reviveBefore.SetActive(false);
        _reviveAfter.SetActive(true);
    }
    #endregion

    #region MeleeAttack
    public void MeleeAttack()
    {
        //내적 이용하여 공격 범위 (전방 부채꼴) 정해서
        Collider[] colliders = Physics.OverlapSphere(transform.position, _monsterData.Range, WhatIsTarget);
        foreach (Collider collider in colliders)
        {
            // 공격 범위 확인
            Vector3 source = transform.position;
            source.y = 0;
            Vector3 destination = collider.transform.position;
            destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            if (targetAngle > _monsterData.Angle * 0.5f)
                continue;

            IDamagable damageble = collider.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.TakeDamage(_monsterData.Damage);
            }
        }
    }
    #endregion

    #region RangeAttack
    public void ThrowAttack()
    {
        GameObject projectile = Object.Instantiate(_projectile, _muzzlePoint.position, _muzzlePoint.rotation);
    }
    #endregion
}
