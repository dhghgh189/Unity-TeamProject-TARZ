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

    [Header("Attack")]
    [SerializeField] float _angle;
    public float Angle { get { return _angle; } set { _angle = value; } }

    [SerializeField] float _range;
    public float Range { get { return _range; } set { _range = value; } }

    private PooledObject _pooledObject;

    public override void OnAwake()
    {
        _monsterData = GetComponent<MonsterData>();
        _animator = GetComponent<Animator>();
        _pooledObject = GetComponent<PooledObject>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_distance <= _monsterData.AttackRange)
        {
            if (attackRoutine == null)
            {
                attackRoutine = StartCoroutine(AttackRoutine());
            }
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    #region 근접 공격
    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_monsterData.MeleeAttackSpeed);
        attackRoutine = null;
    }
    #endregion
}