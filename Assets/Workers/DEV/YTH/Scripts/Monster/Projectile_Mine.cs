using UnityEngine;

public class Projectile_Mine : MonoBehaviour
{
     MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _bombZombie;

    [SerializeField] Rigidbody _rigidBody;

    [SerializeField] float Timer;

    private void Start()
    {
        _monsterSkillManager = _bombZombie.GetComponent<MonsterSkillManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rigidBody.velocity = Vector3.zero;

        Destroy(gameObject, Timer);
        // 밟으면 터질지
        // 타이머로 터질지

        // 이건 정해주시면 수정!!

        IDamagable damagableObj = collision.gameObject.GetComponent<IDamagable>();
        if (damagableObj != null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _monsterSkillManager.BombSkill.Range);
            foreach (Collider collider in colliders)
            {
                // 공격 범위 확인
                Vector3 source = transform.position;
                source.y = 0;
                Vector3 destination = collider.transform.position;
                destination.y = 0;

                Vector3 targetDir = (destination - source).normalized;
                float targetAngle = Vector3.Angle(transform.forward, targetDir);
                if (targetAngle > _monsterSkillManager.BombSkill.Angle) 
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


