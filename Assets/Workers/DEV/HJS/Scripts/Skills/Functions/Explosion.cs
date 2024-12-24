using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, ISpec
{
    [SerializeField] float damage;  // 공격 데미지
    [SerializeField] float range;   // 공격 범위

    public void SetSpec(BaseSkillSO.Spec spec, int level)
    {
        damage = spec.Power(level);
        range = spec.Range(level);  
        Init();
    }

    private void Init()
    {
        Debug.Log("폭팔시작");
        Destroy(gameObject);
    }

    private void OnDestroy()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
