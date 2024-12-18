using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTester : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField] float angle;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    public void Attack()
    {
        // 전방 앞에 있는 몬스터들을 확인하고 피격을 진행해 준다.

        // 1. 범위 안에 몬스터들을 확인
        // NonAlloc 버전을 사용하면 최적화면에서 도움될 수 있음
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider col in colliders)
        {
            // 2. 탐지된 몬스터가 판정을 위한 각도 내에 있는지 확인해야 한다.
            // y의 경우 연산에 포함하지 않는게 좋다.
            Vector3 source = transform.position;
            source.y = 0;   // y를 0으로

            Vector3 dest = col.transform.position;
            dest.y = 0;     // y를 0으로

            Vector3 targetDir = (dest - source).normalized;

            // 플레이어 정면으로부터 몬스터의 방향으로 향하는 각도를 구한다.
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            // 좌측 angle, 우측 angle에 대해 반절씩 확인해야 하므로
            // 0.5를 곱해 angle의 절반만큼의 값과 비교한다.
            if (targetAngle > angle * 0.5f)
                continue;

            IDamagable damagable = col.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // 거리 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        // 각도 그리기
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
    }
}
