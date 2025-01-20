using UnityEngine;

public class ScrapChildObject : MonoBehaviour
{
    [SerializeField] ScrapParentObject parent;
    [SerializeField] float range;
    [SerializeField] ParticleSystem ps;
    [SerializeField] ObjectMoveDestroy sc;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        sc = GetComponentInParent<ObjectMoveDestroy>();
    }

    private void Start()
    {
        parent = GetComponentInParent<ScrapParentObject>();

        if (parent == null) Destroy(this);

        sc.maxTime = parent.Duration;

        //parent.OnStartEvent.AddListener(ps.Play);
        //parent.OnEndEvent.AddListener(ps.Stop);
    }

    private void OnParticleCollision(GameObject other)
    {
        MonsterData monsterData = other.GetComponent<MonsterData>();
        if (other.GetComponent<MonsterData>() is null)
        {
            monsterData = other.GetComponentInParent<MonsterData>();
        }

        parent.ApplyEffect(monsterData);
    }
}
