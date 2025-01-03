using UnityEngine;

public class Projectile : MonoBehaviour // 일반 원딜 쫄몹
{
    [SerializeField] GameObject _radiation;

    IDamagable damagable;

    private Rigidbody _rigidBody;

    private MonsterData _monsterData;

    private void Awake()
    {
        _monsterData = GetComponentInParent<MonsterData>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidBody.AddForce((transform.forward + transform.up) * _monsterData.ThrowPower, ForceMode.Impulse);

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


