using System.Collections;
using UnityEngine;

/// <summary>
/// 블랙홀 오브젝트에 부착하는 스크립트
/// </summary>
public class BlackHollObject : MonoBehaviour
{
    [SerializeField] GameObject body;           // 보여줄 구체

    [Header("발사")]
    [SerializeField] float moveSpeed;           // 움직이는 속도
    [SerializeField] float moveTime;            // 움직이는 시간

    [Header("흡수")]
    [SerializeField] float absorptionRange;     // 흡수 범위
    [SerializeField] float absorptionMinRange;  // 흡수 최소 범위
    [SerializeField] float absorptionMaxRange;  // 흡수 최대 범위
    [SerializeField] float absorptionSpeed;     // 흡수하는 속도

    [Header("폭발")]
    [SerializeField] float explosionRange;      // 폭발 범위
    [SerializeField] float explosionMinDamage;  // 폭발 최소 데미지
    [SerializeField] float explosionMaxDamage;  // 폭발 최대 데미지

    public bool CanThrow;                       // 1초가 지난 시점을 알려주는 변수 <- 던질 수 있다.
    public bool FullCharge;                     // 최대 시간이 모두 지나면 알려주는 변수 <- 던져야 한다.

    private float delta;                        // 변화량

    private float explosionDamage;              // 폭발 데미지

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
        // 충전하기
        coroutine = StartCoroutine(StartChargingRoutine());
    }

    private IEnumerator StartChargingRoutine()
    {
        while (delta <= 2f)
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

    /// <summary>
    /// 던지는 함수
    /// </summary>
    public void Throw()
    {
        // 기존 흡수하는 코루틴을 중단하고
        StopCoroutine(coroutine);
        // 플레이어가 바라보는 방향 -> 구체의 앞 방향으로 속도만큼 이동
        rigid.velocity = dir * moveSpeed;
        // 날아가는 시간을 계산하는 코루틴 실행
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

    // 시간 딜레이 코루틴
    private IEnumerator StartThrowRoutine()
    {
        // 날아가는 시간만큼 기다린 다음
        yield return Util.GetDelay(moveTime);

        // 해당 구체 폭발하기
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
