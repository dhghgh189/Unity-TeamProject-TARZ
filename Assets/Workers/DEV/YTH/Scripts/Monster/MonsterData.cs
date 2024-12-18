using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MonsterData : MonoBehaviour
{
    //Melee : 근거리 공격 몹 //Range : 원거리 공격 몹 //Boss : 보스 몹

    // 몹 종류별로 데이터 3개로 분할할것
    // MeleeData, RangeData, BossData, EleteData
    public enum MonsterType { Melee, Range, Boss }

    [SerializeField] int _maxHp;
    public int MaxHp { get { return _maxHp; } private set { } }

    [SerializeField] int _curHp;
    public int CurHp { get { return _curHp; } set { _curHp = value; } }

    [SerializeField] float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } private set { } }

    [SerializeField] int _damage;
    public int Damage { get { return _damage; } private set { } }

    [SerializeField] float _meleeAttackSpeed;
    public float MeleeAttackSpeed { get {return _meleeAttackSpeed; } set { _meleeAttackSpeed = value; } }

    [SerializeField] float _rangeAttackSpeed;
    public float RangeAttackSpeed { get { return _rangeAttackSpeed; } set { _rangeAttackSpeed = value; } }

    [SerializeField] float _traceRange;
    public float TraceRange { get { return _traceRange; } private set { } }

    [SerializeField] float _attackRange;
    public float AttackRange { get { return _attackRange; } private set { } }

    [SerializeField] float _throwPower;
    public float ThrowPower { get { return _throwPower; } set { _throwPower = value; } }

    [SerializeField] bool _isAttacked; // 피격 상태 (잠시 경직)
    public bool IsAttacked { get { return _isAttacked; } set { _isAttacked = value; } }

    [SerializeField] bool _attacked_First; // 선빵 맞아서 캐릭터 추격하는 변수
    public bool Attacked_First { get { return _attacked_First; } set { _attacked_First = value; } }

    [SerializeField] bool _canUseSkill = true;
    public bool CanUseSkill { get { return _canUseSkill; } set { _canUseSkill = value; } }

    [SerializeField] MonsterType _type;
    public MonsterType Type { get { return _type; } private set { } }

    private static readonly int idleHash = Animator.StringToHash("Idle");
}





