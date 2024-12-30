using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;
using System.Collections;

public class ActMeleeAttack : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] Animator _animator;

    [SerializeField] GameObject _player;

    private float _distance;

    [Header("Attack")]
    [SerializeField] float _angle;
    public float Angle { get { return _angle; } set { _angle = value; } }

    [SerializeField] float _range;
    public float Range { get { return _range; } set { _range = value; } }

    public override void OnStart()
    {
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