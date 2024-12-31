using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;
using System.Collections;

public class ActRangeAttack : Action
{
    private MonsterData _monsterData;
    private Animator _animator;

    private PooledObject _pooledObject;

    private PlayerController _player;

    [SerializeField] SharedGameObject _projectilePrefab;

    [SerializeField] SharedTransform _muzzlePoint;

    private float _distance;

    public override void OnAwake()
    {
        _pooledObject = GetComponent<PooledObject>();
        _monsterData = GetComponent<MonsterData>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
	{
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance <= _monsterData.AttackRange && throwRoutine == null)
        {
            MonsterRotation();
            throwRoutine = StartCoroutine(ThrowRoutine());
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    #region 원거리 공격
    Coroutine throwRoutine;
    IEnumerator ThrowRoutine()
    {
        ThrowAttack();
        //_animator.SetTrigger("Throw");
        yield return new WaitForSeconds(_monsterData.RangeAttackSpeed);
        throwRoutine = null;
    }

    public void ThrowAttack()
    {
        GameObject projectile = Object.Instantiate(_projectilePrefab.Value, _muzzlePoint.Value.position, _muzzlePoint.Value.rotation);
    }
    #endregion

    public void MonsterRotation()
    {
        transform.LookAt(_player.transform);
    }
}