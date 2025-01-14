using System.Collections;
using UnityEngine;

public class SpikeTrap : Trap
{
    [SerializeField] private float damage;
    [SerializeField] private float interval;

    [Header("가시 충돌 판정 용")]
    [SerializeField] private CollisionHandler spikeCollision;

    private BoxCollider coll;
    private Animator anim;

    protected override void Init()
    {
        coll = GetComponent<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        base.Init();
    }

    private void Start()
    {
        spikeCollision.OnTriggerEntered += CheckCollision;
    }

    private void OnDestroy()
    {
        spikeCollision.OnTriggerEntered -= CheckCollision;
    }

    public override void Activate()
    {
        isActive = true;
        coll.enabled = true;
    }

    public override void Deactivate()
    {
        isActive = false;

        if (rebootRoutine != null)
        {
            StopCoroutine(rebootRoutine);
            rebootRoutine = null;
        }

        coll.enabled = false;
    }

    private void CheckCollision(Collider other)
    {
        if (!isActive)
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable == null)
            return;

        Debug.Log($"<color=yellow>충돌 감지 : {damage}</color>");
        damagable.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive
            || rebootRoutine != null)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("<color=red>Spike Trap 발동!</color>");
            coll.enabled = false;
            anim.SetTrigger("Triggered");
        }
    }

    public void Reboot()
    {
        Debug.Log("<color=red>Spike Trap 대기</color>");
        rebootRoutine = StartCoroutine(RebootRoutine());
    }

    private Coroutine rebootRoutine;

    private IEnumerator RebootRoutine()
    {
        yield return Util.GetDelay(interval);
        coll.enabled = true;

        Debug.Log("<color=red>Spike Trap 재활성화</color>");

        rebootRoutine = null;
    }
}
