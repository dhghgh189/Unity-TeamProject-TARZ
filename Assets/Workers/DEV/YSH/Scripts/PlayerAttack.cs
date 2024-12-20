using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform throwPoint;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("원거리 공격 스펙 설정")]
    public ThrowAttackInfo[] ThrowAttackInfo;

    [Header("근거리 공격 스펙 설정")]
    public MeleeAttackInfo[] MeleeAttackInfo;

    [Space(10f)]
    [SerializeField] private Transform stackTransform;
    [SerializeField] private int maxObjectCount;
    [SerializeField] private float comboCheckTime;
    [SerializeField] private PlayerController player;

    private Vector3 source;
    private Vector3 dest;
    private float resultAngle;

    private Stack<ThrowObject> objectStack;
    public int ObjectCount => objectStack.Count;

    public LayerMask WhatIsEnemy { get { return whatIsEnemy; } }

    public EMultiActionType ActionType { get; set; }
    public int MeleeCount { get; set; }
    public int ThrowCount { get; set; }
    public int ThrowCountMax => ThrowAttackInfo.Length;
    public int MeleeCountMax => MeleeAttackInfo.Length;

    private List<IEffect> meleeEffects;
    public int MeleeEffectCount => meleeEffects.Count;
    public float ComboCheckTime => comboCheckTime;

    private EffectGenerator generator;

    private void Awake()
    {
        MeleeCount = 0;
        ThrowCount = 0;

        objectStack = new Stack<ThrowObject>(maxObjectCount);
        meleeEffects = new List<IEffect>();

        generator = new EffectGenerator();

        GenerateThrowEffects();
        GenerateMeleeEffects();
    }

    public void GenerateThrowEffects()
    {
        EffectInfo throwEffectInfo;
        for (int i = 0; i < ThrowAttackInfo.Length; i++)
        {
            // 해당 타수 공격이 멀티 액션인 경우
            if (ThrowAttackInfo[i].MultiActions.Length > 0)
            {
                GenerateMultiActionEffects(i, ThrowAttackInfo[i].MultiActions);
                continue;
            }

            // 단일 액션인 경우
            throwEffectInfo = ThrowAttackInfo[i].EffectInfo;
            if (throwEffectInfo.EffectDatas.Length <= 0)
                continue;

            foreach (var effectData in throwEffectInfo.EffectDatas)
            {
                effectData.Effect = generator.Create(effectData.EffectType);
                Debug.Log($"<color=green>원거리 {i + 1}타 Effect 생성 : {effectData.EffectType}</color>");
            }
        }
    }

    public void GenerateMultiActionEffects(int throwCount, MultiActionInfo[] multiActions)
    {
        EffectInfo multiEffectInfo;
        for (int i = 0; i < multiActions.Length; i++)
        {
            // MultiAction[i]에 적용되야 하는 Effect 정보를 확인
            multiEffectInfo = multiActions[i].EffectInfo;
            if (multiEffectInfo.EffectDatas.Length <= 0)
                continue;

            // 확인된 Effect를 생성
            foreach (var effectData in multiEffectInfo.EffectDatas)
            {
                effectData.Effect = generator.Create(effectData.EffectType);
                Debug.Log($"<color=cyan>원거리 {throwCount + 1}타({multiActions[i].ActionType}) Effect 생성 : {effectData.EffectType}</color>");
            }
        }
    }

    public void GenerateMeleeEffects()
    {
        EffectInfo meleeEffectInfo;
        for (int i = 0; i < MeleeAttackInfo.Length; i++)
        {
            // MeleeAttack[i]에 적용되야 하는 Effect 정보를 확인
            meleeEffectInfo = MeleeAttackInfo[i].EffectInfo;
            if (meleeEffectInfo.EffectDatas.Length <= 0)
                continue;

            // 확인된 Effect를 생성
            foreach (var effectData in meleeEffectInfo.EffectDatas)
            {
                effectData.Effect = generator.Create(effectData.EffectType);
                Debug.Log($"<color=lime>근거리 {i + 1}타 Effect 생성 : {effectData.EffectType}</color>");
            }
        }
    }

    public void AddThrowEffects(ThrowObject tobj)
    {
        // 적용되야 할 Effect가 존재하는지 확인
        if (ThrowAttackInfo[ThrowCount].EffectInfo.EffectDatas.Length <= 0)
            return;

        EffectInfo throwEffectInfo = ThrowAttackInfo[ThrowCount].EffectInfo;
        foreach (var effectData in throwEffectInfo.EffectDatas)
        {
            // 확인된 Effect 적용 
            tobj.AddEffect(effectData.Effect);
        }
    }

    public void AddMultiActionEffects(ThrowObject tobj, MultiActionInfo currentAction)
    {
        // 적용되야 할 Effect가 존재하는지 확인
        if (currentAction.EffectInfo.EffectDatas.Length <= 0)
            return;

        EffectInfo currentActionEffectInfo = currentAction.EffectInfo;
        foreach (var effectData in currentActionEffectInfo.EffectDatas)
        {
            // 확인된 Effect 적용
            tobj.AddEffect(effectData.Effect);
        }
    }

    public void AddMeleeEffect()
    {
        // 적용되야 할 Effect가 존재하는지 확인
        if (MeleeAttackInfo[MeleeCount].EffectInfo.EffectDatas.Length <= 0)
            return;

        EffectInfo meleeEffectInfo = MeleeAttackInfo[MeleeCount].EffectInfo;
        foreach (var effectData in meleeEffectInfo.EffectDatas)
        {
            // 확인된 Effect 적용
            meleeEffects.Add(effectData.Effect);
        }
    }

    public void ActiveMeleeEffect(GameObject target)
    {
        foreach (var effect in meleeEffects)
        {
            // 적용된 Melee Effect 실행
            effect.Activate(gameObject, target);
        }

        // 실행 후 초기화
        meleeEffects.Clear();
    }

    public void ClearMeleeEffects()
    {
        meleeEffects.Clear();
    }

    public void AddObjectStack(ThrowObject tobj)
    {
        if (objectStack.Count >= maxObjectCount)
            return;

        objectStack.Push(tobj);
        tobj.transform.parent = stackTransform;
        tobj.gameObject.SetActive(false);

        // 이벤트?
        //Debug.Log($"<color=yellow>Stack Count : {ObjectCount}</color>");
    }

    public ThrowObject PopObjectStack()
    {
        if (objectStack.Count <= 0)
        {
            Debug.Log("물건이 없습니다.");
            return null;
        }

        ThrowObject tobj = objectStack.Pop();

        // 이벤트?
        //Debug.Log($"<color=yellow>Stack Count : {ObjectCount}</color>");

        return tobj;
    }

    public void Throw()
    {
        // 최종 수치 저장용
        int damage = 0;
        float throwForce = 0;

        ThrowObject tobj = PopObjectStack();
        if (tobj == null)
            return;

        Debug.Log($"<color=white>Throw Count : {ThrowCount}</color>");

        MultiActionInfo[] multiActions = ThrowAttackInfo[ThrowCount].MultiActions;
        MultiActionInfo currentAction;
        if (multiActions.Length <= 0)
        {
            AddThrowEffects(tobj);

            // 수치 저장
            damage = ThrowAttackInfo[ThrowCount].Damage;
            throwForce = ThrowAttackInfo[ThrowCount].ThrowForce;
        }
        else
        {
            List<MultiActionInfo> actionList = multiActions.ToList();
            // 현재 Action과 일치하는 MultiActionInfo를 가져온다 (Where)
            currentAction = actionList.Where(x => x.ActionType == ActionType).First();
            AddMultiActionEffects(tobj, currentAction);

            // 수치 저장
            damage = currentAction.Damage;
            throwForce = currentAction.ThrowForce;
        }

        tobj.transform.parent = null;
        tobj.transform.position = throwPoint.position;
        tobj.gameObject.SetActive(true);
        tobj.SetDamage(damage);
        tobj.Throw(transform.forward + (transform.up * 0.3f), throwForce);
    }

    public void Melee()
    {
        Debug.Log($"MeleeCount : {MeleeCount}");
        Debug.Log($"Melee Attack angle : {MeleeAttackInfo[MeleeCount].Angle}");
        Debug.Log($"Melee Attack Range : {MeleeAttackInfo[MeleeCount].Range}");
        Debug.Log($"Melee Attack Damage : {MeleeAttackInfo[MeleeCount].Damage}");

        AddMeleeEffect();

        Collider[] colliders = Physics.OverlapSphere(transform.position, MeleeAttackInfo[MeleeCount].Range, whatIsEnemy);
        foreach (Collider col in colliders)
        {
            // 각도 체크
            if (!IsTargetInAngle(col.transform))
                continue;

            // 이펙트 발동
            ActiveMeleeEffect(col.gameObject);

            IDamagable damagable = col.GetComponent<IDamagable>();
            if (damagable == null)
                continue;

            damagable.TakeDamage(MeleeAttackInfo[MeleeCount].Damage);
        }

        if (MeleeEffectCount > 0)
            ClearMeleeEffects();

        if (MeleeCount < MeleeCountMax - 1)
            MeleeCount++;
        else
            MeleeCount = 0;
    }

    private bool IsTargetInAngle(Transform targetTrf)
    {
        source = transform.position;    // 플레이어 위치
        dest = targetTrf.position;      // 감지된 Target 위치

        // y축 수치 제거
        source.y = 0;
        dest.y = 0;

        // 플레이어와 Target간의 각도 체크
        resultAngle = Vector3.Angle(transform.forward, (dest - source).normalized);
        if (resultAngle > MeleeAttackInfo[MeleeCount].Angle * 0.5f)
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MeleeAttackInfo[MeleeCount].Range);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position,
            (Quaternion.Euler(0, MeleeAttackInfo[MeleeCount].Angle * 0.5f, 0) * transform.forward) * MeleeAttackInfo[MeleeCount].Range);
        Gizmos.DrawRay(transform.position,
            (Quaternion.Euler(0, MeleeAttackInfo[MeleeCount].Angle * -0.5f, 0) * transform.forward) * MeleeAttackInfo[MeleeCount].Range);
    }
}
