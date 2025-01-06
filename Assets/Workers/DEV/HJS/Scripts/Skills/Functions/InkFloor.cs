using UnityEngine;

/// <summary>
/// 먹물 장판
/// </summary>
public class InkFloor : MonoBehaviour, ISpec
{
    private float operationTime;
    private Interaction interaction;
    private SphereCollider coll;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    public void SetSpec(Spec spec, int level)
    {
        coll.radius = spec.Range(level);
        operationTime = spec.Time(level);
        interaction = new Interaction(SkillEnum.InteractionType.Slow);
        interaction.SetSpec(spec, level);
        Init();
    }

    private void Init()
    {
        Debug.Log("잉크영역 시작");
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
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, coll.radius);
    }
}
