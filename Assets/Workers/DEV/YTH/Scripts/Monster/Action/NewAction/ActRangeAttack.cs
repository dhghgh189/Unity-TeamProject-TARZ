using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;
using System.Collections;

public class ActRangeAttack : Action
{
    [Inject]
    private CoroutineManager _util;

    [SerializeField] MonsterData _monsterData;

    [SerializeField] Animator _animator;

    [SerializeField] GameObject _player;

    [SerializeField] SharedGameObject _projectilePrefab;

    [SerializeField] SharedTransform _muzzlePoint;

    private float _distance;

    public override void OnStart()
	{
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance <= _monsterData.AttackRange)
        {
            _util.StartRoutine(ref throwRoutine, ThrowRoutine());
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
}