using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 고철 덩어리 오브젝트에 부착하는 스크립트
/// </summary>
public class ScrapMatelObject : MonoBehaviour
{
    [SerializeField] GameObject body;

    [Header("발사")]
    [SerializeField] float force;           // 힘
    [SerializeField] float maxTime;         // 모으는 최대 시간
    [SerializeField] float damage;          // 데미지 
    [SerializeField] SphereCollider coll;
    [SerializeField] Rigidbody rigid;
    private Coroutine coroutine;

    public Vector3 dir;

    private float delta;            // 변화량
    public bool IsFull;

    private void Awake()
    {
        delta = 0f;
        coll = GetComponent<SphereCollider>();
        coll.enabled = false;
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
        IsFull = false;
    }

    private void Charge()
    {
        // 충전하기
        coroutine = StartCoroutine(StartChargingRoutine());
    }

    private IEnumerator StartChargingRoutine()
    {
        while (delta <= maxTime)
        {
            body.transform.localScale = Vector3.one * delta;
            coll.radius = delta * 0.5f;
            delta += Time.deltaTime;
            yield return null;
        }
        body.transform.localScale = Vector3.one * maxTime;
        coll.radius = maxTime * 0.5f;

        IsFull = true;
    }

    public void Throw()
    {
        rigid.isKinematic = false;
        rigid.useGravity = true;
        // 힘만큼 던져버리기
        rigid.AddForce(dir * force * maxTime, ForceMode.Impulse);
        // Collider 활성화
        coll.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))        // 몬스터에 닿을경우 -> 피격 데미지
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(damage); }
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))    // 땅에 닿을경우 -> 삭제
        {
            Destroy(gameObject);
        }
    }

    public void Init(BagSkillDataSO skilldata)
    {
        // 데이터 추가
        damage = skilldata.Getdata((int)BagSkillEnum.CompactCanonDataType.DefaultDamage).value;
        maxTime = skilldata.Getdata((int)BagSkillEnum.CompactCanonDataType.ChargeTime).value;
        force = skilldata.Getdata((int)BagSkillEnum.CompactCanonDataType.Force).value;

        Charge();
    }
}
