using UnityEngine;
using Zenject;

/// <summary>
/// 폭발을 담당하는 스크립트
/// </summary>
public class Explosion : MonoBehaviour, ISpec
{
    [SerializeField] float damage;  // 공격 데미지
    [SerializeField] float range;   // 공격 범위
    [SerializeField] SphereCollider sphere;

    public void SetSpec(Spec spec, int level)
    {
        damage = spec.Power(level);
        range = spec.Range(level);

        sphere.radius = range;

        Init();
    }

    private void Awake()
    {
        sphere = GetComponent<SphereCollider>();
    }

    private void Init()
    {
        Debug.Log("폭팔시작");

        if(transform.parent == null)
        {
            // Overlap으로 범위 확인
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Monster"));
            // 모든 적에게 데미지 입히기 방송
            foreach (Collider collider in colliders)
            {
                IDamagable damagable = collider.GetComponent<IDamagable>();
                if (damagable != null) { damagable.TakeDamage(damage); }
            }
        }
        else
            sphere.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
