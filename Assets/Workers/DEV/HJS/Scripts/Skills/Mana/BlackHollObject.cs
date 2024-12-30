using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using UnityEngine;

public class BlackHollObject : MonoBehaviour
{
    [SerializeField] GameObject body;

    [Header("발사")]
    [SerializeField] float moveSpeed;
    [SerializeField] float moveTime;

    [Header("흡수")]
    [SerializeField] float absorptionRange;
    [SerializeField] float absorptionMinRange;
    [SerializeField] float absorptionMaxRange;
    [SerializeField] float absorptionSpeed;

    [Header("폭발")]
    [SerializeField] float explosionRange;
    [SerializeField] float explosionMinDamage;
    [SerializeField] float explosionMaxDamage;

    public bool CanThrow;
    public bool FullCharge;

    private float delta;

    private float explosionDamage;
    private float damageDelta;
    private float rangeDelta;

    [SerializeField] SphereCollider coll;
    [SerializeField] Rigidbody rigid;
    public Vector3 dir;

    private Coroutine coroutine;

    private void Awake()
    {
        CanThrow = false;
        FullCharge = false;
        delta = 0f;
        coll = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Charge();
    }


    private void Charge()
    {
       // TODO: 충전하기 (1~2초 동안 크기 키우기 + 끌어당기기)
       coroutine = StartCoroutine(StartChargingRoutine());
    }

    private IEnumerator StartChargingRoutine()
    {
        while(delta <= 2f)
        {
            if (delta >= 1f) CanThrow = true;
            
            absorptionRange = Mathf.Clamp(absorptionMinRange * delta, absorptionMinRange, absorptionMaxRange);
            explosionDamage = Mathf.Clamp(explosionMinDamage * delta, explosionMinDamage, explosionMaxDamage);
            body.transform.localScale = Vector3.one * delta * 0.5f;

            coll.radius = absorptionRange;

            delta += Time.deltaTime;
            yield return null;
        }

        body.transform.localScale = Vector3.one;
        absorptionRange = absorptionMaxRange;
        explosionDamage = explosionMaxDamage;

        coll.radius = absorptionRange;

        FullCharge = true;
    }

    public void Throw()
    {
        StopCoroutine(coroutine);
        rigid.velocity = dir * moveSpeed;
        StartCoroutine(StartThrowRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            Debug.Log("StartHolidng");
            StartCoroutine(StartBoilingRoutine(other.gameObject.transform));
        }
    }

    private IEnumerator StartThrowRoutine()
    {
        float time = 0f;
        while (time < moveTime)
        {
            yield return null;
            time += Time.deltaTime;
        }

        Destroy(gameObject);
    }

    private IEnumerator StartBoilingRoutine(Transform other)
    {
        while (true)
        {
            // 거리를 계산하고
            Vector3 relativeDirection = other.position - transform.position;

            // 정규화를 진행하고
            Vector3 gravityDirection = relativeDirection.normalized;

            // 현재 있는 오브젝트 방향으로 힘의 량만큼 끌어당기기
            other.gameObject.GetComponent<Rigidbody>().velocity = -gravityDirection * absorptionSpeed;

            // 흡수하고 있을 때 데미지
            yield return new WaitForFixedUpdate();
        }
    }

    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(explosionDamage); Debug.Log($"{collider.gameObject.name}에게 {explosionDamage}만큼의 피해를 입혔다!"); }
        }
    }

    private void OnDestroy()
    {
        Explosion();
        StopAllCoroutines();
    }

    public void Init(ManaSkillDataSO data)
    {
        // 값 설정
        moveSpeed = data.GetData((int)ManaBlackHollDataType.ThrowSpeed);
        moveTime = data.GetData((int)ManaBlackHollDataType.FlightTime);
        absorptionMinRange = data.GetData((int)ManaBlackHollDataType.AbsorptionMinRange);
        absorptionMaxRange = data.GetData((int)ManaBlackHollDataType.AbsorptionMaxRange);
        absorptionSpeed = data.GetData((int)ManaBlackHollDataType.AbsorptionSpeed);
        explosionMinDamage = data.GetData((int)ManaBlackHollDataType.ExplosionMinDamage);
        explosionMaxDamage = data.GetData((int)ManaBlackHollDataType.ExplosionMaxDamage);
        explosionRange = data.GetData((int)ManaBlackHollDataType.ExplosionRange);

        // 값 설정후 모으기 시작
        Charge();
    }
}
