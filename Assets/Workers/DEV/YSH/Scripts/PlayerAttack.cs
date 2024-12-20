using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float[] meleeAngles;
    [SerializeField] private float[] meleeRanges;
    [SerializeField] private EffectInfo[] meleeEffectInfo;

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

    public int MeleeCount { get; set; }
    public int ThrowCount { get; set; }
    public int ThrowCountMax => ThrowAttackInfo.Length;
    public int MeleeCountMax => meleeAngles.Length;

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

        // Melee Effect 생성
        foreach (var effectInfo in meleeEffectInfo)
        {
            for (int i = 0; i < effectInfo.EffectDatas.Length; i++)
                effectInfo.EffectDatas[i].Effect = generator.Create(effectInfo.EffectDatas[i].EffectType);
        }
    }

    public void GenerateThrowEffects()
    {
        EffectInfo throwEffectInfo;
        for (int i = 0; i < ThrowAttackInfo.Length; i++)
        {
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

    public void AddThrowEffects(ThrowObject tobj)
    {
        if (ThrowAttackInfo[ThrowCount].EffectInfo.EffectDatas.Length <= 0)
            return;

        EffectInfo throwEffectInfo = ThrowAttackInfo[ThrowCount].EffectInfo;
        foreach (var effectData in throwEffectInfo.EffectDatas)
        {
            tobj.AddEffect(effectData.Effect);
        }
    }

    public void AddMeleeEffect()
    {
        if (meleeEffectInfo[MeleeCount].ExistEffet)
        {
            foreach (var effectData in meleeEffectInfo[MeleeCount].EffectDatas)
            {
                meleeEffects.Add(effectData.Effect);
            }
        }
    }

    public void ActiveMeleeEffect(GameObject target)
    {
        foreach (var effect in meleeEffects)
        {
            effect.Activate(gameObject, target);
        }

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
        Debug.Log($"<color=yellow>Stack Count : {ObjectCount}</color>");
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
        Debug.Log($"<color=yellow>Stack Count : {ObjectCount}</color>");

        return tobj;
    }

    public void Throw()
    {
        ThrowObject tobj = PopObjectStack();
        if (tobj == null)
            return;

        Debug.Log($"<color=white>Throw Count : {ThrowCount}</color>");

        AddThrowEffects(tobj);

        tobj.transform.parent = null;
        tobj.transform.position = throwPoint.position;
        tobj.gameObject.SetActive(true);
        tobj.SetDamage(ThrowAttackInfo[ThrowCount].Damage);
        tobj.Throw(transform.forward + (transform.up*0.3f), ThrowAttackInfo[ThrowCount].ThrowForce);

        // 공격 시 추후 카메라 방향을 바라보도록 하는 동작 추가 필요 

        if (ThrowCount < ThrowCountMax - 1)
            ThrowCount++;
        else
            ThrowCount = 0;
    }

    public void Melee()
    {
        Debug.Log($"MeleeCount : {MeleeCount}");
        Debug.Log($"Melee Attack angle : {meleeAngles[MeleeCount]}");
        Debug.Log($"Melee Attack Range : {meleeRanges[MeleeCount]}");
        Debug.Log($"Melee Attack Damage : {player.Stat.MeleeDamages[MeleeCount]}");

        AddMeleeEffect();

        // 공격 시 추후 카메라 방향을 바라보도록 하는 동작 추가 필요 

        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRanges[MeleeCount], whatIsEnemy);
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

            damagable.TakeDamage(player.Stat.MeleeDamages[MeleeCount]);
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
        source = transform.position;
        dest = targetTrf.position;
        source.y = 0;
        dest.y = 0;

        resultAngle = Vector3.Angle(transform.forward, (dest - source).normalized);
        if (resultAngle > meleeAngles[MeleeCount] * 0.5f)
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRanges[MeleeCount]);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position,
            (Quaternion.Euler(0, meleeAngles[MeleeCount] * 0.5f, 0) * transform.forward) * meleeRanges[MeleeCount]);
        Gizmos.DrawRay(transform.position,
            (Quaternion.Euler(0, meleeAngles[MeleeCount] * -0.5f, 0) * transform.forward) * meleeRanges[MeleeCount]);
    }
}
