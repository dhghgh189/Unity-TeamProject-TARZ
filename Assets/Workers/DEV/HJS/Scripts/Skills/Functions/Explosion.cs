using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, ISpec
{
    [SerializeField] float damage;  // 공격 데미지

    public void SetSpec(SkillSpecDatabase.Spec spec, int level)
    {
        damage = spec.Power(level);

        Init();
    }

    private void Init()
    {
        Debug.Log("폭팔시작");
        // Overlap으로 범위 확인
        // 모든 적에게 데미지 입히기 방송
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
