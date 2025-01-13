using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScrapParentObject : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float duration;
    [SerializeField] float distance;
    [SerializeField] float waitDelay;

    private Dictionary<GameObject, bool> effectableDic = new Dictionary<GameObject, bool>();
    [HideInInspector] public UnityEvent OnStartEvent;
    [HideInInspector] public UnityEvent OnEndEvent;
    [SerializeField] ParticleSystem chargePs;
    [SerializeField] GameObject scrap;

    private void Awake()
    {
        OnStartEvent = new UnityEvent();
        OnEndEvent = new UnityEvent();
    }

    private void Start()
    {
        StartCoroutine(LifeCycleRoutine());
    }

    private IEnumerator LifeCycleRoutine()
    {
        yield return Util.GetDelay(waitDelay);
        OnStartEvent?.Invoke();
        chargePs.Stop();

        yield return Util.GetDelay(duration);

        OnEndEvent?.Invoke();

        Destroy(gameObject);
    }

    public void ApplyEffect(MonsterData other)
    {
        if (!effectableDic.TryAdd(other.gameObject, true) || other.IsDead)
        {
            return;
        }

        // 한번 넉백 당하고 데미지 입기
        other.gameObject.GetComponent<IKnockBack>()?.KnockBack(gameObject);
        other.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage);
    }

    private void OnDestroy()
    {
        effectableDic.Clear();
        OnStartEvent.RemoveAllListeners();
    }

    public void Init(BagSkillDataSO data)
    {
        damage = data.Getdata((int)BagSkillEnum.ScrapBurstDataType.DefaultDamage).value;
        duration = data.Getdata((int)BagSkillEnum.ScrapBurstDataType.Duration).value;
        distance = data.Getdata((int)BagSkillEnum.ScrapBurstDataType.Distance).value;
        waitDelay = data.Getdata((int)BagSkillEnum.ScrapBurstDataType.WaitDelay).value;
    }
}
