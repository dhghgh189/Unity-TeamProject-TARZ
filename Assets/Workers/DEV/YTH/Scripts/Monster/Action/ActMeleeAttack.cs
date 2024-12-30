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
                Debug.Log("Attack Routine Start");
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

    //public void Attack()
    //{
    //    Debug.Log("Attack 함수 시작");
    //    //내적 이용하여 공격 범위 (전방 부채꼴) 정해서
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, _range);
    //    foreach (Collider collider in colliders)
    //    {
    //        // 공격 범위 확인
    //        Vector3 source = transform.position;
    //        source.y = 0;
    //        Vector3 destination = collider.transform.position;
    //        destination.y = 0;

    //        Vector3 targetDir = (destination - source).normalized;
    //        float targetAngle = Vector3.Angle(transform.forward, targetDir);
    //        if (targetAngle > _angle * 0.5f)
    //            continue;

    //        Debug.Log("공격 조건 OK");
    //        IDamagable damageble = collider.GetComponent<IDamagable>();
    //        if (damageble != null)
    //        {
    //            Debug.Log("공격");
    //            damageble.TakeDamage(_monsterData.Damage);
    //        }
    //    }
    //}
    #endregion
}