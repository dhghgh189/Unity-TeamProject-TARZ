using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MonsterData : MonoBehaviour
{
    //Melee : �ٰŸ� ���� �� //Range : ���Ÿ� ���� �� //Boss : ���� ��

    // �� �������� ������ 3���� �����Ұ�
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

    [SerializeField] float _traceRange;
    public float TraceRange { get { return _traceRange; } private set { } }

    [SerializeField] float _attackRange;
    public float AttackRange { get { return _attackRange; } private set { } }

    [SerializeField] bool _isAttacked; // �ǰ� ����
    public bool IsAttacked { get { return _isAttacked; } set { _isAttacked = value; } }

    [SerializeField] bool _useSkill = false;
    public bool UseSkill { get { return _useSkill; } set { _useSkill = value; } }

    [SerializeField] MonsterType _type;
    public MonsterType Type { get { return _type; } private set { } }

    private static readonly int idleHash = Animator.StringToHash("Idle");
}





