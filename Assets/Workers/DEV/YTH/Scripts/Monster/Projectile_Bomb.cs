using UnityEngine;

public class Projectile_Bomb : MonoBehaviour
{
     MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _bombZombie;

    private void Start()
    {
        _monsterSkillManager = _bombZombie.GetComponent<MonsterSkillManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        // ������� ���� �����
        // ���� �ø��� ��ź ��ü�� �߿�x ��ǥ ������ �߿�����
        // or ��ü�� ���� �� ���� ������� ���������� 

        IDamagable damagableObj = collision.gameObject.GetComponent<IDamagable>();
        if (damagableObj != null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _monsterSkillManager.BombSkill.Range);
            foreach (Collider collider in colliders)
            {
                // ���� ���� Ȯ��
                Vector3 source = transform.position;
                source.y = 0;
                Vector3 destination = collider.transform.position;
                destination.y = 0;

                Vector3 targetDir = (destination - source).normalized;
                float targetAngle = Vector3.Angle(transform.forward, targetDir);
                if (targetAngle > _monsterSkillManager.BombSkill.Angle) // �ޱ��� ������
                    continue;

                IDamagable damageble = collider.GetComponent<IDamagable>();
                if (damageble != null)
                {
                    damageble.TakeDamage(_monsterSkillManager.BombSkill.Damage);
                }
            }
        }
    }
}


