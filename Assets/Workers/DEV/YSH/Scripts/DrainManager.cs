using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DrainManager : MonoBehaviour
{
    [SerializeField] private float maxRadius;
    [SerializeField] private float drainSpeed;
    [SerializeField] LayerMask whatIsTarget;
    [SerializeField] PlayerController player;
    [SerializeField] private ParticleSystem drainEffect;

    private SphereCollider col;
    private float minRadius;

    private Coroutine drainRoutine;
    private ParticleSystem cachedEffect;

    public PlayerController Player => player;
    public float DrainSpeed => drainSpeed;
    public float MaxRadius { get => maxRadius; set { maxRadius = value; } }

    public float DrainStaminaAmount;

    private float scaleDelta = 2.5f;

    [HideInInspector] public UnityEvent OnStartEffectEvent = new(); // 드레인에 있는 능력의 효과를 시작시키는 이벤트
    [HideInInspector] public UnityEvent OnStopEffectEvent = new();  // 드레인에 있는 능력의 효과를 중단시키는 이벤트

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        minRadius = col.radius;

        // 최초에 이펙트 미리 생성
        cachedEffect = Instantiate(drainEffect, col.transform);
    }

    public void StartDrain()
    {
        if (drainRoutine != null)
            return;

        OnStartEffectEvent?.Invoke();
        drainRoutine = StartCoroutine(DrainRoutine());
    }

    IEnumerator DrainRoutine()
    {
        col.radius = minRadius;
        cachedEffect.transform.localScale = Vector3.one * (minRadius / scaleDelta);

        // 콜라이더를 원점으로 이동
        transform.localPosition = Vector3.zero;

        while (true)
        {
            if (col.radius < maxRadius)
            {
                col.radius = Mathf.MoveTowards(col.radius, maxRadius, drainSpeed * Time.deltaTime);
                cachedEffect.transform.localScale = Vector3.one * (col.radius / scaleDelta);
            }
            else
            {
                col.radius = maxRadius;
                cachedEffect.transform.localScale = Vector3.one * (maxRadius / scaleDelta);
                yield break;
            }

            yield return null;
        }
    }

    public void StopDrain()
    {
        // 콜라이더를 게임 영역 외로 보내버린다.
        // 비활성화 하는것으로는 Exit 호출이 안되므로 물리적으로 벗어나게 하기 위함
        col.radius = minRadius;
        cachedEffect.transform.localScale = Vector3.one * (minRadius / scaleDelta);
        transform.localPosition = Vector3.up * 100f;
        OnStopEffectEvent?.Invoke();

        if (drainRoutine != null)
            StopCoroutine(drainRoutine);

        drainRoutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDrainable drainable = other.GetComponent<IDrainable>();
        if (drainable != null)
        {
            drainable.DoDrain(this);
        }
        player.SkillHandler.ActivateSkill(EState.Drain, SkillEnum.ActionTimingType.Act, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        IDrainable drainable = other.GetComponent<IDrainable>();
        if (drainable != null)
        {
            drainable.StopDrain(this);
        }
    }
}
