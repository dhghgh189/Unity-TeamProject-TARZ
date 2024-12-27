using System.Collections;
using UnityEngine;

public class Projectile_Bomb : MonoBehaviour
{
    [SerializeField] GameObject _bombZombie;

    [SerializeField] SphereCollider _sphereCollider;

    private MonsterSkillManager _monsterSkillManager;

    private void Start()
    {
        _monsterSkillManager = _bombZombie.GetComponent<MonsterSkillManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(CountDown());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player"))
            return;

        IDamagable damagableObj = other.gameObject.GetComponent<IDamagable>();
        damagableObj.TakeDamage(_monsterSkillManager.BombSkill.Damage);
    }

    IEnumerator CountDown()
    {
        yield return Util.GetDelay(3f);
        _sphereCollider.enabled = true;
        yield return Util.GetDelay(0.1f);
        Destroy(gameObject);
    }
}


