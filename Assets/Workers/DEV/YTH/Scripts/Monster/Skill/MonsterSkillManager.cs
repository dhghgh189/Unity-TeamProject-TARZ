using System.Collections;
using UnityEngine;

// 제안 1) 쌩으로 함수 구현해서 스킬매니저에 몰아 놓는다
// 제안 2) abstract
// 제안 3) 
/// <summary>
/// 1. 기본스킬 + @
/// 2. 투사체 스킬 + @   
/// 3. 지점공격 (ex> 전기떨어트림) + @
/// 4. 대시 공격 + @
/// 애니메이션만 교체해서하면될듯
/// </summary>
/// 
public class MonsterSkillManager : MonoBehaviour
{
    #region Skill-ScriptableObj
    [Header("MonsterSkill")]
    [SerializeField] MonsterSkill _bomb;
    public MonsterSkill BombSkill { get { return _bomb; } set { _bomb = value; } }

    [SerializeField] MonsterSkill _dashAttack;
    public MonsterSkill DashAttackSkill { get { return _dashAttack; } set { _dashAttack = value; } }

    [SerializeField] MonsterSkill _firePoison_Wall;
    public MonsterSkill FirePoisonWallSkill { get { return _firePoison_Wall; } set { _firePoison_Wall = value; } }

    [SerializeField] MonsterSkill _jumpAttack;
    public MonsterSkill JumpAttackSkill { get { return _jumpAttack; } set { _jumpAttack = value; } }

    [SerializeField] MonsterSkill _mine;
    public MonsterSkill MineSkill { get { return _mine; } set { _mine = value; } }

    [SerializeField] MonsterSkill _stimPak;
    public MonsterSkill StimPakSkill { get { return _stimPak; } set { _stimPak = value; } }

    [SerializeField] MonsterSkill _wheelWind;
    public MonsterSkill WheelWindSkill { get { return _wheelWind; } set { _wheelWind = value; } }

    [SerializeField] MonsterSkill _electricWall;
    public MonsterSkill ElectricWallSkill { get { return _electricWall; } set { _electricWall = value; } }

    [SerializeField] MonsterSkill _thunder;
    public MonsterSkill ThunderSkill { get { return _thunder; } set { _thunder = value; } }
    #endregion

    [Header("Prefab")]
    [SerializeField] GameObject _bombPrefab;

    [SerializeField] GameObject _minePrefab;

    [SerializeField] GameObject _jackTheRipper;

    [SerializeField] GameObject _electricWallPrefab;

    [SerializeField] GameObject _thunderPrefab;


    private Vector3 _electricWallPosition;
    private Vector3 _electricWallPosition2;

    [Header("")]
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _player;

    [SerializeField] Rigidbody _rigidbody;

    private void Start()
    {
        SkillInit();
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
        DashAttackSkill.CanUseSkill = true;
    }

    private void Update() // 테스트 코드 추후 삭제!!
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

        }
    }

    #region JumpAttack
    public Coroutine jumpAttackRoutine;
    public IEnumerator JumpAttackRoutine() // 보스의 도약해서 착지하여 범위 공격
    {
        _rigidbody.AddForce((Vector3.forward + Vector3.up * 2f) * _jumpAttack.JumpForce, ForceMode.Impulse);
        //점프가 안됨 내브매쉬랑 연관있을것으로 추정
        Debug.Log("점프!!");
        /*  _monsterData.CanUseSkill = false;*/
        _jumpAttack.CanUseSkill = false;

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

        yield return new WaitForSeconds(_jumpAttack.CoolTime);
        /*  _monsterData.CanUseSkill = true;*/
        _jumpAttack.CanUseSkill = true;
        jumpAttackRoutine = null;
    }
    #endregion

    #region WheelWind
    public Coroutine wheelWindRoutine;
    public IEnumerator WheelWindRoutine() // 가렌 E
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _wheelWind.Range);
        foreach (Collider collider in colliders)
        {
            // 공격 범위 확인
            Vector3 source = transform.position;
            source.y = 0;
            Vector3 destination = collider.transform.position;
            destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            if (targetAngle > _wheelWind.Angle) // 앵글의 반절만
                continue;

            IDamagable damageble = collider.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.TakeDamage(_wheelWind.Damage);
                yield return new WaitForSeconds(_wheelWind.Interval);
            }
        }
    }
    #endregion

    #region Bomb
    public Coroutine bombRoutine;
    public IEnumerator BombRoutine()  // 직스 궁 
    {
        BombSkill.CanUseSkill = false;

        GameObject bomb = Instantiate(_bombPrefab, transform.position, transform.rotation);
        Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
        bombRb.AddForce(Vector3.forward * _bomb.ThrowForce, ForceMode.Impulse);

        yield return new WaitForSeconds(BombSkill.CoolTime);
        bombRoutine = null;
        BombSkill.CanUseSkill = true;

    }
    #endregion

    #region Mine
    public Coroutine mineRoutine;
    public IEnumerator MineRoutine()
    {
        MineSkill.CanUseSkill = false;

        GameObject mine = Instantiate(_minePrefab, transform.position, transform.rotation);
        Rigidbody mineRb = mine.GetComponent<Rigidbody>();
        mineRb.AddForce(Vector3.forward * _bomb.ThrowForce, ForceMode.Impulse);

        yield return Util.GetDelay(MineSkill.CoolTime);
        mineRoutine=null;
        MineSkill.CanUseSkill=true;
    }
    #endregion

    #region StimPak
    public  void StimPak() // 폭탄좀비가 잭더리퍼의 몬스터 데이터에 접근해서 스텟 업 해줌
    {
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
    WaitForSeconds dashAttackCoolTime = new(10f);
    public IEnumerator DashAttackRoutine()
    {
        /*_monsterData.CanUseSkill = false;*/
        _rigidbody.AddForce(Vector3.forward * _dashAttack.JumpForce, ForceMode.Impulse);
        // 물리기반으로할지, translate로 할지 결정
        // rigidbody 사용 시 캐릭터에 부딪히면 멈춤
        // 뚫고 나가는게 맞는거같기도하고 물어봐야함

        Collider[] colliders = Physics.OverlapSphere(transform.position, _dashAttack.Range);
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
        yield return dashAttackCoolTime;
        /*_monsterData.CanUseSkill = true;*/

    }
    #endregion

    #region ElectricWall
    public Coroutine electricWallRoutine;
    public IEnumerator ElectricWallRoutine()
    {
        ElectricWallSkill.CanUseSkill = false;
        _electricWallPosition = new Vector3(transform.position.x, 0, transform.position.z + 5f);
        GameObject electricWall = Instantiate(_electricWallPrefab, _electricWallPosition, transform.rotation);

        for (int i = 0; i < 6; i++)
        {
            yield return Util.GetDelay(ElectricWallSkill.Interval);
            _electricWallPosition2 = new Vector3(electricWall.transform.position.x, 0, electricWall.transform.position.z + (i * 7f));
            GameObject electricWall2 = Instantiate(_electricWallPrefab, _electricWallPosition2, transform.rotation);
        }

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

}

