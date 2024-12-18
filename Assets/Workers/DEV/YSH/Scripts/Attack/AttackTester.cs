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
        // ���� �տ� �ִ� ���͵��� Ȯ���ϰ� �ǰ��� ������ �ش�.

        // 1. ���� �ȿ� ���͵��� Ȯ��
        // NonAlloc ������ ����ϸ� ����ȭ�鿡�� ����� �� ����
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider col in colliders)
        {
            // 2. Ž���� ���Ͱ� ������ ���� ���� ���� �ִ��� Ȯ���ؾ� �Ѵ�.
            // y�� ��� ���꿡 �������� �ʴ°� ����.
            Vector3 source = transform.position;
            source.y = 0;   // y�� 0����

            Vector3 dest = col.transform.position;
            dest.y = 0;     // y�� 0����

            Vector3 targetDir = (dest - source).normalized;

            // �÷��̾� �������κ��� ������ �������� ���ϴ� ������ ���Ѵ�.
            float targetAngle = Vector3.Angle(transform.forward, targetDir);
            // ���� angle, ���� angle�� ���� ������ Ȯ���ؾ� �ϹǷ�
            // 0.5�� ���� angle�� ���ݸ�ŭ�� ���� ���Ѵ�.
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
        // �Ÿ� �׸���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        // ���� �׸���
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
    }
}
