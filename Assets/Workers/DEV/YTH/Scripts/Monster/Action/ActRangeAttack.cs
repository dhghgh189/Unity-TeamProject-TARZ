using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;

public class ActRangeAttack : Action
{
    [SerializeField] Transform _muzzlePoint;

    private MonsterData _monsterData;

    private MonsterSkillManager _monsterSkillManager;

    private Animator _animator;

    private PooledObject _pooledObject;

    private PlayerController _player;

    private float _distance;

    public override void OnAwake()
    {
        _pooledObject = GetComponent<PooledObject>();
        _monsterData = GetComponent<MonsterData>();
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        _muzzlePoint = transform.Find("MuzzlePoint");
        _monsterSkillManager.MuzzlePoint = _muzzlePoint;
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance <= _monsterData.AttackRange && throwRoutine == null)
        {
            MonsterRotation();
            throwRoutine = StartCoroutine(ThrowRoutine());
            _animator.SetTrigger("Attack");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    Coroutine throwRoutine;
    IEnumerator ThrowRoutine()
    {
        yield return new WaitForSeconds(_monsterData.RangeAttackSpeed);
        throwRoutine = null;
    }

    public void MonsterRotation()
    {
        transform.LookAt(_player.transform);
    }
}