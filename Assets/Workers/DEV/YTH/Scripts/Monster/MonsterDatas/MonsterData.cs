using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class MonsterData : MonoBehaviour
{
    public enum MonsterType { Melee, Range, Boss, Bomb, Frog, Revive }  //Melee : 근거리 공격 몹 //Range : 원거리 공격 몹 //Boss : 아놀드 //Baomber : 폭탄좀비
    [SerializeField] MonsterType _monsterType;
    public MonsterType MonsterTyPe { get { return _monsterType; } private set { } }

    public enum SkillType { Skill, UnSkill }
    [SerializeField] SkillType _skillType;
    public SkillType SkillTyPe { get { return _skillType; } set { } }

    public enum MonsterTier { Normal, Elite, Boss }
    [SerializeField] MonsterTier _monsterTier;
    public MonsterTier MonsterTIer { get { return _monsterTier; } private set { } }


    [SerializeField] float _maxHp;
    public float MaxHp { get { return _maxHp; } private set { } }

    [SerializeField] float _curHp;
    public float CurHp { get { return _curHp; } set { _curHp = value; } }

    [SerializeField] float _damage;
    public float Damage { get { return _damage; }  set { _damage = value; } }

    [SerializeField] float _attackRange; // 공격 거리
    public float AttackRange { get { return _attackRange; }  set { _attackRange = value; } }

    [SerializeField] float _attackSpeed; // 공격 속도
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    private bool _isAttacked; // 피격 상태 (잠시 경직)
    public bool IsAttacked { get { return _isAttacked; } set { _isAttacked = value; } }

    private bool _isCatched; // 잡힌 상태 (이동 불가)
    public bool IsCatched { get { return _isCatched; } set { _isCatched = value; } }

    private bool _isDead; // 사망 시
    public bool IsDead { get { return _isDead;  }  set { _isDead = value; } }

    private bool _canSkill;
    public bool CanSkill { get { return _canSkill; } set { _canSkill = value; } }

    public bool IsCountered { get; set; }

    [Header("근거리 평타 각도 (거리는 AttackRange)")]
    [SerializeField] float _angle;
    public float Angle { get { return _angle; } set { _angle = value; } }
   
    [Header("폭탄 좀비")]
    [SerializeField] float _canUseProjectileSkillDistance; // 폭탄 좀비 스킬 가능 거리
    public float CanUseProjectileSkillDistance { get { return _canUseProjectileSkillDistance; } set { _canUseProjectileSkillDistance = value; } }

    [SerializeField] float _dangerDistance;
    public float DangerDistance { get { return _dangerDistance; } set { _dangerDistance = value; } }

    [Header("개구리 점프 좀비")]
    [SerializeField] float _canJumpDistance;
    public float CanJumpDistance { get { return _canJumpDistance; } set { _canJumpDistance = value; } }

    [Header("Sound ID")]
    [SerializeField] int _attackID;
    public int AttackID { get { return _attackID; } }

    [SerializeField] int _takeDamageID;
    public int TakeDamageID { get { return _takeDamageID; } }

    [SerializeField] int _dieID;
    public int DieID { get { return _dieID; } }

    [SerializeField] int spawnID;
    public int SpawnID { get { return spawnID; } }

    public CapsuleCollider coll { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigid { get; private set; }
    public PooledObject pooledObject { get; private set; }
    
    private void Awake()
    {
        CurHp = MaxHp;
        coll = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        pooledObject = GetComponent<PooledObject>();
        CanSkill = true;
    }
}





