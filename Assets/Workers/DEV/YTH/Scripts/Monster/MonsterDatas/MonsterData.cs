using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class MonsterData : MonoBehaviour
{
    public enum MonsterType { Melee, Range, Boss, Bomb, Frog }  //Melee : 근거리 공격 몹 //Range : 원거리 공격 몹 //Boss : 아놀드 //Baomber : 폭탄좀비
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
    public float Damage { get { return _damage; } private set { } }

    [SerializeField] float _attackRange; // 공격 실행 가능 범위
    public float AttackRange { get { return _attackRange; } private set { } }

    [SerializeField] bool _isAttacked; // 피격 상태 (잠시 경직)
    public bool IsAttacked { get { return _isAttacked; } set { _isAttacked = value; } }

    [SerializeField] bool _attacked_First; // 선빵 맞아서 캐릭터 추격하는 변수
    public bool Attacked_First { get { return _attacked_First; } set { _attacked_First = value; } }

    [SerializeField] float _range;
    public float Range { get { return _range; } set { _range = value; } }
    [SerializeField] float _angle;
    public float Angle { get { return _angle; } set { _angle = value; } }

    [Header("근거리 몬스터")]
    [SerializeField] float _meleeAttackSpeed; // 근접 공격 속도
    public float MeleeAttackSpeed { get { return _meleeAttackSpeed; } set { _meleeAttackSpeed = value; } }

    [Header("원거리 몬스터")]
    [SerializeField] float _rangeAttackSpeed; // 원거리 공격 속도
    public float RangeAttackSpeed { get { return _rangeAttackSpeed; } set { _rangeAttackSpeed = value; } }

    [SerializeField] float _throwPower; // 일반 원딜 몬스터 일반 공격 던지는 힘
    public float ThrowPower { get { return _throwPower; } set { _throwPower = value; } }

    [SerializeField] float _dangerDistance;
    public float DangerDistance { get { return _dangerDistance; } set { _dangerDistance = value; } }

    [Header("폭탄 좀비")]
    [SerializeField] float _canUseProjectileSkillDistance; // 폭탄 좀비 스킬 가능 거리
    public float CanUseProjectileSkillDistance { get { return _canUseProjectileSkillDistance; } set { _canUseProjectileSkillDistance = value; } }

    private static readonly int jake_Move_Hash = Animator.StringToHash("Jake_Move");

    [Header("개구리 점프 거리")]
    [SerializeField] float _canJumpDistance;
    public float CanJumpDistance { get { return _canJumpDistance; } set { _canJumpDistance = value; } }

    private void Awake()
    {
        CurHp = MaxHp;
    }
}





