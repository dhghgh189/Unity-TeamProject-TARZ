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
        _sphereCollider.radius = _monsterSkillManager.BombSkill.Range;
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(CountDown());
        Debug.Log("바닥에 닿음");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player")) return;

        Debug.Log("플레이어가 맞았다");
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


