using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.GridLayoutGroup;

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
    [Header("CreateThrowObejct_Init")]
    [SerializeField] ThrowObject instance;
    [SerializeField] float count;
    [SerializeField] float radius;
    [SerializeField] float force;
    [SerializeField] List<GameObject> throwObjectList;

    [SerializeField] Rigidbody rigid;
    [SerializeField] BoxCollider coll;
    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();

        coll.enabled = false;
        rigid.useGravity = false;
        throwObjectList = new List<GameObject>();
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
            // 이펙트 생성
            EffectManager.instance.ParticlePlay("ManaSkill_21", 1f, transform.position, Quaternion.identity);
            // SFX 재생
            SoundManager.PlaySFX(SoundManager.SoundData_S.ManaSkillSounds_2[1].AudioClip);
            // 던지는 물체 생성            
            CreateThrowObject();
            // 기능을 다한 차량 오브젝트는 삭제
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 초기 값을 설정하는 함수
    /// </summary>
    /// <param name="data">해당 스킬의 데이터</param>
    public void Init(ManaSkillDataSO data, float skillDamagePercent)
    {
        speed = data.GetData((int)ManaThrowCarDataType.FlightSpeed);
        hitDamage = data.GetData((int)ManaThrowCarDataType.HitDamage) * skillDamagePercent;
        explosionDamage = data.GetData((int)ManaThrowCarDataType.ExplosionDamage) * skillDamagePercent;
        explosionRange = data.GetData((int)ManaThrowCarDataType.ExplosionRange);
        count = data.GetData((int)ManaThrowCarDataType.CreateThrowObject);
    }

    private void CreateThrowObject()
    {
        // 던지는 물체의 갯수
        Vector3 dir = Vector3.zero;
        Vector3 pos = transform.position;

        // 던지는 물체 원형으로 생성
        for (int i = 0; i < count; i++)
        {
            float angle = i * (Mathf.PI * 2.0f) / count;

            GameObject child = Instantiate(instance, pos, Quaternion.identity).gameObject;
            throwObjectList.Add(child);
            child.transform.position
                = pos + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))) * radius + Vector3.up;

            dir = child.transform.position - pos;
            child.transform.rotation = Quaternion.LookRotation(dir.normalized);
        }

        // Addforce로 날리기
        foreach (GameObject item in throwObjectList)
        {
            item.GetComponent<Rigidbody>().AddForce((item.transform.forward) * force, ForceMode.Impulse);
        }

    }
}
