using System.Collections;
using UnityEngine;

/// <summary>
/// 던지는 차에 부착하는 스크립트
/// </summary>
public class ThrowCarObject : MonoBehaviour
{
    [Header("Init")]
    [SerializeField] ParticleSystem effect;     // TODO: 충돌일 생겼을 때 발생할 파티클
    [SerializeField] float speed;               // 날아가는 속도
    [SerializeField] float hitDamage;           // 피격을 입히는 데미지
    [SerializeField] float explosionDamage;     // 폭발했을 때 데미지
    [SerializeField] float explosionRange;      // 폭발하는 범위

    [SerializeField] Rigidbody rigid;
    [SerializeField] BoxCollider coll;
    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();

        coll.enabled = false;
        rigid.useGravity = false;
    }

    /// <summary>
    /// 던지기 시작하는 함수
    /// </summary>
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
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))    // 몬스터에 닿을경우 -> 피격 데미지
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(hitDamage); }
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))    // 땅에 닿을경우 -> 폭파
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, LayerMask.GetMask("Monster"));
            foreach (Collider collider in colliders)
            {
                IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
                if (damagable != null) { damagable.TakeDamage(explosionDamage); Debug.Log($"{collider.gameObject.name}에게 {explosionDamage}만큼의 피해를 입혔다!"); }
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 초기 값을 설정하는 함수
    /// </summary>
    /// <param name="data">해당 스킬의 데이터</param>
    public void Init(ManaSkillDataSO data)
    {
        speed = data.GetData((int)ManaThrowCarDataType.FlightSpeed);
        hitDamage = data.GetData((int)ManaThrowCarDataType.HitDamage);
        explosionDamage = data.GetData((int)ManaThrowCarDataType.ExplosionDamage);
        explosionRange = data.GetData((int)ManaThrowCarDataType.ExplosionRange);
    }

}
