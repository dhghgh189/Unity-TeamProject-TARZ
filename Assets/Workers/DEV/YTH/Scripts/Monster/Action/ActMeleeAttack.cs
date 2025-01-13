using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;
using System.Collections;

public class ActMeleeAttack : Action
{
    private MonsterData _monsterData;

    private Animator _animator;

    private PlayerController _player;

    private float _distance;

    private PooledObject _pooledObject;

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
        if (_distance <= _monsterData.AttackRange && !_monsterData.IsCatched)
        {
            if (attackRoutine == null)
            {
                _animator.SetBool("Move", false);
                attackRoutine = StartCoroutine(AttackRoutine());
                _animator.SetTrigger("Attack");
                SoundManager.PlaySFX(SoundManager.SoundData_M.MeleeAttack);
            }
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(_monsterData.AttackSpeed);
        attackRoutine = null;
    }
}