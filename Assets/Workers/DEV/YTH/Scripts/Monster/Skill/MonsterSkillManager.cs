using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] MonsterSkill _firePoison_Wall;
    [SerializeField] MonsterSkill _jumpAttack;
    [SerializeField] MonsterSkill _mine;
    [SerializeField] MonsterSkill _stimPak;
    [SerializeField] MonsterSkill _wheelWind;
    [SerializeField] MonsterSkill _electricWall;
    #endregion

    [Header("Prefab")]
    [SerializeField] GameObject _bombPrefab;

    [SerializeField] GameObject _minePrefab;

    [SerializeField] GameObject _jackTheRipper;

    [SerializeField] GameObject _wallPrefab;

    [Header("")]
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _player;

    [SerializeField] Rigidbody _rigidbody;

   

    WaitForSeconds attackDelay = new(2f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(JumpAttackRoutine());
        }
    }

    public Coroutine jumpAttackRoutine;
    WaitForSeconds jumpAttackCoolTime = new(10f);
    public IEnumerator JumpAttackRoutine() // 보스의 도약해서 착지하여 범위 공격
    {
        _rigidbody.AddForce((Vector3.forward + Vector3.up * 2f) * _jumpAttack.JumpForce, ForceMode.Impulse);
        Debug.Log("점프!!");
        _monsterData.CanUseSkill = false;

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
        
        yield return jumpAttackCoolTime;
        _monsterData.CanUseSkill = true;
        jumpAttackRoutine = null;
    }


    public IEnumerator WheelWind() // 가렌 E
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
                yield return attackDelay;
            }
        }
    }

    WaitForSeconds bombCoolTime = new(10f);
    public IEnumerator Bomb()  // 직스 궁 
    {
        _monsterData.CanUseSkill = false;
        GameObject bomb = Instantiate(_bombPrefab, transform.position, transform.rotation);
        Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
        bombRb.AddForce(Vector3.forward * _bomb.ThrowForce, ForceMode.Impulse);
        yield return bombCoolTime;
        _monsterData.CanUseSkill = true;
    }

    public void Mine()
    {
        GameObject mine = Instantiate(_minePrefab, transform.position, transform.rotation);
        Rigidbody mineRb = mine.GetComponent<Rigidbody>();
        mineRb.AddForce(Vector3.forward * _bomb.ThrowForce, ForceMode.Impulse);
    }

    public void FirePoison_Wall()
    {
       //폭탄좀비 위치 기준으로 1자벽 생성 해서 방향은 랜덤 일정 시간뒤에 사라짐
       // 닿으면 데미지입고 화상(도트뎀)입을지 말지 결정은 추후 

        // 벽을 랜덤하게 세울지
        // 캐릭터 위치 기반으로 세울지

        // 한번 세우고말지
        // 여러번 세울지

        GameObject wall = Instantiate(_wallPrefab, transform.position, transform.rotation);
        
    }


    public void StimPak() // 폭탄좀비가 잭더리퍼의 몬스터 데이터에 접근해서 스텟 업 해줌
    {
        if (_player == null)
            return;

        PlayerControllerMonster data = _player.GetComponent<PlayerControllerMonster>();
        data.CurHp += 50;
    }


    public Coroutine dashAttack;
    WaitForSeconds dashAttackCoolTime = new(10f);
    public IEnumerator DashAttack()
    {
        _monsterData.CanUseSkill = false;
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
        _monsterData.CanUseSkill = true;

    }

    public void ElectricWall()
    {
        // 이거 번개 얘기임!!
        // 1자로 세우고
        // 생성 위치 표시하고 몇초뒤 생성, 생성 시점에서 보스에서 멀어지는 방향으로 이동
        // 생성 위치는 수정하고 

    }
}
