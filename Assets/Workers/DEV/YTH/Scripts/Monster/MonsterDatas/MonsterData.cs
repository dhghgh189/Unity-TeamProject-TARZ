using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MonsterData : MonoBehaviour
{
    public enum MonsterType { Melee, Range, Boss, Bomb, Frog }  //Melee : 근거리 공격 몹 //Range : 원거리 공격 몹 //Boss : 아놀드 //Baomber : 폭탄좀비

    [SerializeField] MonsterType _type;
    public MonsterType Type { get { return _type; } private set { } }

    [SerializeField] int _maxHp;
    public int MaxHp { get { return _maxHp; } private set { } }

    [SerializeField] int _curHp;
    public int CurHp { get { return _curHp; } set { _curHp = value; } }

    [SerializeField] float _moveSpeed; // NavMesh랑 연동할것
    public float MoveSpeed { get { return _moveSpeed; } private set { } }

    [SerializeField] int _damage;
    public int Damage { get { return _damage; } private set { } }

    [SerializeField] float _meleeAttackSpeed; // 근접 공격 속도
    public float MeleeAttackSpeed { get {return _meleeAttackSpeed; } set { _meleeAttackSpeed = value; } }

    [SerializeField] float _rangeAttackSpeed; // 원거리 공격 속도
    public float RangeAttackSpeed { get { return _rangeAttackSpeed; } set { _rangeAttackSpeed = value; } }

    [SerializeField] float _attackRange; // 공격 실행 가능 범위
    public float AttackRange { get { return _attackRange; } private set { } }

    [SerializeField] float _throwPower; // 일반 원딜 몬스터 일반 공격 던지는 힘
    public float ThrowPower { get { return _throwPower; } set { _throwPower = value; } }

    [SerializeField] bool _isAttacked; // 피격 상태 (잠시 경직)
    public bool IsAttacked { get { return _isAttacked; } set { _isAttacked = value; } }

    [SerializeField] bool _attacked_First; // 선빵 맞아서 캐릭터 추격하는 변수
    public bool Attacked_First { get { return _attacked_First; } set { _attacked_First = value; } }

    [SerializeField] float _canUseProjectileSkillDistance; // 폭탄 좀비 스킬 가능 거리
    public float CanUseProjectileSkillDistance { get { return _canUseProjectileSkillDistance; } set { _canUseProjectileSkillDistance = value; } }
   

    private static readonly int idleHash = Animator.StringToHash("Idle");
}





