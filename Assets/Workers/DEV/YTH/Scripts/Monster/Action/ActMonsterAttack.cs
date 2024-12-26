using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;

/// <summary>
/// 1. 근접공격
/// 2. 원거리 공격
/// 3. 개구리점프 - 대쉬어택
/// 4. 공격로직 
/// </summary>
public class ActMonsterAttack : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] Animator _animator;

    [SerializeField] SharedGameObject _projectilePrefab;

    [SerializeField] SharedTransform _muzzlePoint;

    [SerializeField] GameObject _player;

    private float _distance;

    [Header("Attack")]
    [SerializeField] float _angle;
    public float Angle { get { return _angle; } set { _angle = value; } }

    [SerializeField] float _range;
    public float Range { get { return _range; } set { _range = value; } }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_distance <= _monsterData.AttackRange)
        {
            // 근딜, 원딜 몬스터 공격 로직 분리
            switch (_monsterData.MonsterTyPe)
            {
                case MonsterData.MonsterType.Range:
                case MonsterData.MonsterType.Bomb:
                    if (throwRoutine == null)
                    {
                        throwRoutine = StartCoroutine(ThrowRoutine());
                        Debug.Log("throw루틴 했음");
                    }
                    break;

                case MonsterData.MonsterType.Frog:

                    if (_distance > 5f && _monsterSkillManager.frogJumpAttackRoutine == null)
                    {
                        _monsterSkillManager.frogJumpAttackRoutine = StartCoroutine(_monsterSkillManager.FrogJumpAttackRoutine());
                        Debug.Log("개구리 점프!!");
                    }
                    else if (attackRoutine == null)
                    {
                        attackRoutine = StartCoroutine(AttackRoutine());
                        Debug.Log("근접공격루틴했음");
                    }
                    break;

                default:
                    if (attackRoutine == null)
                    {
                        attackRoutine = StartCoroutine(AttackRoutine());
                        Debug.Log("근접공격루틴했음");
                    }
                    break;
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
        Attack(_range, _angle);
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_monsterData.MeleeAttackSpeed);
        attackRoutine = null;
    }

    private void Attack(float range, float angle)
    {
        //내적 이용하여 공격 범위 (전방 부채꼴) 정해서
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders)
        {
            // 공격 범위 확인
            Vector3 source = transform.position;
            source.y = 0;
            Vector3 destination = collider.transform.position;
            destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            if (targetAngle > angle * 0.5f)
                continue;

            IDamagable damageble = collider.GetComponent<IDamagable>();
            if (damageble != null)
            {
                damageble.TakeDamage(_monsterData.Damage);
            }
        }
    }
    #endregion

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
