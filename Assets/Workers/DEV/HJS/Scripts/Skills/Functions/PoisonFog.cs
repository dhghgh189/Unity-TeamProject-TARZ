using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 독 안개를 생성하는 스크립트
/// </summary>
public class PoisonFog : MonoBehaviour, ISpec
{
    private Interaction interaction;
    private SphereCollider coll;
    private float operationTime;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    public void SetSpec(BaseSkillSO.Spec spec, int level)
    {
        coll.radius = spec.Range(level);
        operationTime = spec.Time(level);
        interaction = new Interaction(SkillEnum.InteractionType.Poison);
        interaction.SetSpec(spec, level);
        Init();
    }

    private void Init()
    {
        Debug.Log("독안개 시작");
        Destroy(gameObject, operationTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            interaction.Activate(gameObject, other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, coll.radius);
    }
}
