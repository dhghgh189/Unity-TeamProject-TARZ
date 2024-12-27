using System.Collections;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class ThrowCarObject : MonoBehaviour
{
    [Header("Init")]
    [SerializeField] ParticleSystem effect;
    [SerializeField] float speed;
    [SerializeField] float hitDamage;
    [SerializeField] float explosionDamage;
    [SerializeField] float explosionRange;

    [SerializeField] Rigidbody rigid;
    [SerializeField] BoxCollider coll;
    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();

        coll.enabled = false;
        rigid.useGravity = false;
    }

    public void Throw()
    {
        coll.enabled = true;
        Debug.LogWarning("차 날라가기 시작!");
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        yield return null;
        rigid.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(hitDamage); }
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(explosionDamage); Debug.Log($"{collider.gameObject.name}에게 {explosionDamage}만큼의 피해를 입혔다!"); }
        }
    }

    public void Init(ManaSkillDataSO data)
    {
        speed = data.GetData((int)ManaThrowCarDataType.FlightSpeed);
        hitDamage = data.GetData((int)ManaThrowCarDataType.HitDamage);
        explosionDamage = data.GetData((int)ManaThrowCarDataType.ExplosionDamage);
        explosionRange = data.GetData((int)ManaThrowCarDataType.ExplosionRange);
    }

}
