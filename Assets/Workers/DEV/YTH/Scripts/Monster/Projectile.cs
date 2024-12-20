using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour // 일반 원딜 쫄몹
{
    IDamagable damagable;

    [SerializeField] Rigidbody _rigidBody;

    private MonsterData _monsterData;

    [SerializeField] GameObject _monster;

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

        IDamagable damagableObj = collider.gameObject.GetComponent<IDamagable>();
        damagable = damagableObj;
        if (damagable != null)
        {
            damagable.TakeDamage(5);
        }
    }

    // 도트뎀 코루틴 : 일정 시간마다 데미지를 주기적으로 줌
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


