using System.Collections;
using UnityEngine;

public class Radiation : MonoBehaviour
{
    [SerializeField] float _interval;
    public float Interaval { get { return _interval; } set { _interval = value; } }

    [SerializeField] float _damage;
    public float Damage { get { return _damage; } set { _damage = value; } }

    private MonsterData _monsterData;

    private MonsterSkillManager _skillManager;

    IDamagable _damagable;

    private void Awake()
    {
        _skillManager = GetComponentInParent<MonsterSkillManager>();
        _monsterData = GetComponentInParent<MonsterData>();
    }

    private void Start()
    {
        if (_monsterData.MonsterTIer != MonsterData.MonsterTier.Elite)
            return;

        EffectManager.instance.ParticlePlay("Radiation", 60, transform.position, Quaternion.identity,  transform);
    }

    private void Update()
    {
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamagable damagable = other.GetComponent<IDamagable>();
            _damagable = damagable;

            if (_damagable != null)
            {
                if (takeDOTRoutine == null)
                {
                    takeDOTRoutine = StartCoroutine(TakeDOTRoutine(_damage));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (takeDOTRoutine != null)
        {
            StopCoroutine(takeDOTRoutine);
            takeDOTRoutine = null;
        }
    }

    private void OnDisable()
    {
        if (takeDOTRoutine != null)
        {
            StopCoroutine(takeDOTRoutine);
            takeDOTRoutine = null;
        }
    }

    Coroutine takeDOTRoutine;
    public IEnumerator TakeDOTRoutine(float damage)
    {
        _damagable.TakeDamage(damage);
        yield return new WaitForSeconds(_interval);  // 기획분들이 정해주시면 딜레이 캐싱 해두기
        takeDOTRoutine = null;
    }
}
