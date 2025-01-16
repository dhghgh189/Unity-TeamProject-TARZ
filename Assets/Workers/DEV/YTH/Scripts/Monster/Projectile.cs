using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour // 일반 원딜 쫄몹
{
   [SerializeField] MonsterSkillManager skillManager;

   [SerializeField] Radiation _radiation;

    private Rigidbody _rigidBody;

    IDamagable damagable;

    private PooledObject _pooledObject;

    private PlayerController _player;

    private float _distance;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _pooledObject = GetComponentInParent<PooledObject>();
        skillManager = GetComponentInParent<MonsterSkillManager>();
    }

    private void Start()
    {

        _player = _pooledObject.player;

        _distance = Vector3.Distance(transform.position, _player.transform.position);

        _rigidBody.AddForce((transform.forward + transform.up) * _distance * 0.7f, ForceMode.Impulse);

        transform.parent = null;

        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collider)
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        
        Radiation radiation = Instantiate(_radiation, transform.position, Quaternion.identity);
        radiation.SetParent(skillManager, _pooledObject.MonsterData);
        radiation.ReserveDestroy();
        EffectManager.instance.ParticlePlay("PoisonPool", 3f, transform.position, Quaternion.identity);
        
        if (collider.gameObject.CompareTag("Player"))
        {
            IDamagable damagableObj = collider.gameObject.GetComponent<IDamagable>();
            damagable = damagableObj;
            if (damagable != null)
            {
                damagable.TakeDamage(5);
            }
        }
        Destroy(gameObject);
    }
}


