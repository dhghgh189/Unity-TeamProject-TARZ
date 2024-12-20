using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;

public class ActMonsterAttack : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] Animator _animator;

    [SerializeField] SharedGameObject _projectilePrefab;

    [SerializeField] SharedTransform _muzzlePoint;

    [SerializeField] GameObject _player;

    [Header("Attack")]
    [SerializeField] float _throwForce;
    public float ThrowForce { get { return _throwForce; } set { _throwForce = value; } }

    [SerializeField] float _angle;
    public float Angle { get { return _angle; } set { _angle = value; } }

    [SerializeField] float _range;
    public float Range { get { return _range; } set { _range = value; } }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _monsterData.AttackRange)
        {
            // 근딜, 원딜 몬스터 공격 로직 분리
            switch (_monsterData.Type)
            {
                case MonsterData.MonsterType.Range:
                case MonsterData.MonsterType.Bomb:
                    if (throwRoutine == null)
                    {
                        throwRoutine = StartCoroutine(ThrowRoutine());
                        Debug.Log("throw루틴 했음");
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

    // 공격 사이에 딜레이 생성

    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        Attack(_range, _angle);
        //_animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_monsterData.MeleeAttackSpeed);
        attackRoutine = null;
    }

    Coroutine throwRoutine;
    IEnumerator ThrowRoutine()
    {
        ThrowAttack();
        //_animator.SetTrigger("Throw");
        yield return new WaitForSeconds(_monsterData.RangeAttackSpeed);
        throwRoutine = null;
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
        // 애니메이션에 메서드 추가하기
    }

    public void ThrowAttack()
    {
        GameObject projectile = Object.Instantiate(_projectilePrefab.Value, _muzzlePoint.Value.position, _muzzlePoint.Value.rotation);
    }
}
