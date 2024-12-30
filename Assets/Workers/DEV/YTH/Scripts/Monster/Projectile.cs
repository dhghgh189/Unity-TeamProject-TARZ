using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour // 일반 원딜 쫄몹
{
    IDamagable damagable;

    [SerializeField] Rigidbody _rigidBody;

    [SerializeField] GameObject _monster;

    [SerializeField] GameObject _radiation;

    private MonsterData _monsterData;

    private void Start()
    {
        _monsterData = _monster.GetComponent<MonsterData>(); 

        _rigidBody.AddForce((transform.forward + transform.up) * _monsterData.ThrowPower, ForceMode.Impulse);
    }
    private void Update()
    {
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collider)
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;

        _radiation.SetActive(true);

        IDamagable damagableObj = collider.gameObject.GetComponent<IDamagable>();
        damagable = damagableObj;
        if (damagable != null)
        {
            damagable.TakeDamage(5);
        }
    }
}


