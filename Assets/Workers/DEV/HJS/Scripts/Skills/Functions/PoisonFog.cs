using UnityEngine;

/// <summary>
/// 독 안개를 생성하는 스크립트
/// </summary>
public class PoisonFog : MonoBehaviour, ISpec
{
    [SerializeField] SkillSoundScript script;
    private Interaction interaction;        // 상태이상을 담당하는 클래스
    private SphereCollider coll;            // 독 안개의 감지를 담당할 콜라이더
    private float operationTime;            // 동작하는 시간
    private float damage;
    private WaitQueue damagedQueue;
    private IDamagable target;

    private void Awake()
    {
        script = GetComponent<SkillSoundScript>();
        coll = GetComponent<SphereCollider>();
        damagedQueue = GetComponentInParent<WaitQueue>();
    }

    public void SetSpec(Spec spec, int level)
    {
        transform.localScale = Vector3.one * spec.Range(level);
        operationTime = spec.Time(level);
        damage = spec.Power(level);
        Init();
    }

    private void Init()
    {
        Debug.Log("독안개 시작");
        Destroy(gameObject, operationTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            if (damagedQueue.IsTargetInQueue(other.gameObject)) return;

            target = other.GetComponent<IDamagable>();
            if (target == null) return;

            // 중독 효과음
            script.PlaySound();
            target.TakeDamage(damage);

            damagedQueue.Add(other.gameObject, 1f);
        }
    }
}
