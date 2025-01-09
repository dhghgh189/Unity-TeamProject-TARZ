using System.Collections;
using UnityEngine;

public class SpikeTrap : Trap
{
    [SerializeField] private float damage;
    [SerializeField] private float interval;
    [SerializeField] private float upward;      // 상승 수치
    [SerializeField] private Transform spikes;

    [Header("가시 속도 조정")]
    [SerializeField] private float upSpeed;
    [SerializeField] private float downSpeed;

    [Header("가시 충돌 판정 용")]
    [SerializeField] private CollisionHandler spikeCollision;

    private Vector3 startPos;
    private float rebootTime;

    private BoxCollider coll;

    protected override void Init()
    {
        coll = GetComponent<BoxCollider>();
        base.Init();
        startPos = spikes.position;
    }

    private void Start()
    {
        spikeCollision.OnTriggered += CheckCollision;
    }

    private void OnDestroy()
    {
        spikeCollision.OnTriggered -= CheckCollision;
    }

    public override void Activate()
    {
        isActive = true;
        coll.enabled = true;
    }

    public override void Deactivate()
    {
        isActive = false;

        if (spikeRoutine != null)
        {
            StopCoroutine(spikeRoutine);
            spikeRoutine = null;
            spikes.transform.position = startPos;
        }

        if (rebootRoutine != null)
        {
            StopCoroutine(rebootRoutine);
            rebootRoutine = null;
        }

        coll.enabled = false;
    }

    private void CheckCollision(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable == null)
            return;

        Debug.Log($"<color=yellow>충돌 감지 : {damage}</color>");
        damagable.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive
            || spikeRoutine != null)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("<color=red>Spike Trap 발동!</color>");
            spikeRoutine = StartCoroutine(SpikeRoutine());
        }
    }

    private Coroutine spikeRoutine;
    private Coroutine rebootRoutine;

    private IEnumerator SpikeRoutine()
    {
        coll.enabled = false;

        float currentY = 0;
        Vector3 targetPos = startPos;

        targetPos.y = startPos.y + upward;
        while (true)
        {
            currentY = spikes.position.y;
            if ((targetPos.y - currentY) <= 0.01f)
            {
                spikes.position = targetPos;
                break;
            }

            spikes.transform.position = Vector3.MoveTowards(spikes.position, targetPos, upSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("상승 종료");

        yield return Util.GetDelay(1f);

        targetPos.y = startPos.y;
        while (true)
        {
            currentY = spikes.position.y;
            if ((currentY - targetPos.y) <= 0.01f)
            {
                spikes.position = targetPos;
                break;
            }

            spikes.transform.position = Vector3.MoveTowards(spikes.position, targetPos, downSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("하강 종료");

        spikeRoutine = null;

        Debug.Log("Spike Trap 재활성화 대기...");
        rebootRoutine = StartCoroutine(RebootRoutine());
    }

    private IEnumerator RebootRoutine()
    {
        yield return Util.GetDelay(interval);
        coll.enabled = true;

        Debug.Log("<color=red>Spike Trap 재활성화</color>");

        rebootRoutine = null;
    }
}
