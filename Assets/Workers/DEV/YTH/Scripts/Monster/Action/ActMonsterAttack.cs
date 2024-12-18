using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using UnityEngine;

public class ActMonsterAttack : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] Animator _animator;

    [SerializeField] SharedGameObject _projectilePrefab;

    [SerializeField] SharedTransform _muzzlePoint;

    [SerializeField] GameObject target;

    [SerializeField] float _throwForce; 
    public float ThrowForce { get { return _throwForce; } set { _throwForce = value; } }

    [SerializeField] float _angle;
    public float Angle { get { return _angle; } set { _angle = value; } }

    [SerializeField] float _range;
    public float Range { get { return _range; } set { _range = value; } }

    public override TaskStatus OnUpdate()
    {
        
        if (Vector3.Distance(transform.position, target.transform.position) < _monsterData.AttackRange)
        {
            // 근딜, 원딜 몬스터 공격 로직 분리
            switch (_monsterData.Type)
            {
                case MonsterData.MonsterType.Range:
                    if (throwRoutine == null)
                    {
                        throwRoutine = StartCoroutine(ThrowRoutine());
                    }
                    break;

                default:
                    if (attackRoutine == null)
                    {
                        attackRoutine = StartCoroutine(AttackRoutine());
                    }
                    break;
            }
            Debug.Log("공격했음~~~~~~~~~");
            return TaskStatus.Success;
        }
        else
        {
           /* StopCoroutine("attackRoutine");*/
           /* StopCoroutine("throwRoutine");*/
            return TaskStatus.Failure;
        }
    }

   // 공격 사이에 딜레이 생성
   WaitForSeconds attackDelay = new(1f);
   WaitForSeconds throwDelay = new(2f);

    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        Attack(_range, _angle);
        //_animator.SetTrigger("Attack");
        yield return attackDelay;
        attackRoutine = null;
    }

    Coroutine throwRoutine;
    IEnumerator ThrowRoutine()
    {
        ThrowAttack();
        //_animator.SetTrigger("Throw");
        //공격 속도만큼 딜레이 주기
        // 아니면 던진 투사체가 사라지는 시간만큼 딜레이
        yield return throwDelay;
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
        // 애니메이션에 메서드 추가하기
    }

    public void ThrowAttack()
    {
        GameObject projectile = Object.Instantiate(_projectilePrefab.Value, _muzzlePoint.Value.position, _muzzlePoint.Value.rotation);
        Rigidbody projectileRb = GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(Vector3.forward + Vector3.up * _throwForce, ForceMode.Impulse);
        }
    }
}
