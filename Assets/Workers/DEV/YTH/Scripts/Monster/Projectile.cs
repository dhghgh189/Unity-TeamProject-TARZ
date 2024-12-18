using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour // �Ϲ� ���� �̸�
{
    IDamagable damagable;

    private void Update()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerStay(Collider collider)
    {
        IDamagable damagableObj = collider.GetComponent<IDamagable>();
        damagable = damagableObj;
        if (damagable != null)
        {
            damagable.TakeDamage(5);
        }
    }

    // ��Ʈ�� �ڷ�ƾ : ���� �ð����� �������� �ֱ������� ��
    WaitForSeconds attackDelay = new(2f);
    public IEnumerator TakeDOTRoutine(int damage)
    {
        while (true)
        {
            damagable.TakeDamage(damage);
            yield return attackDelay;
        }
    }
}


