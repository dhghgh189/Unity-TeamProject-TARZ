using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform throwPoint;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float[] throwForces;
    [SerializeField] private float[] meleeAngles;
    [SerializeField] private float[] meleeRanges;
    [SerializeField] private Transform stackTransform;
    [SerializeField] private int maxObjectCount;

    private Vector3 source;
    private Vector3 dest;
    private float resultAngle;

    private Stack<ThrowObject> objectStack;
    public int ObjectCount => objectStack.Count;

    public LayerMask WhatIsEnemy { get { return whatIsEnemy; } }

    public int MeleeCount { get; set; }
    public int ThrowCount { get; set; }
    public int ThrowCountMax => throwForces.Length;
    public int MeleeCountMax => meleeAngles.Length;

    private List<IEffect> meleeEffects;
    public int MeleeEffectCount => meleeEffects.Count;

    private void Awake()
    {
        MeleeCount = 0;
        ThrowCount = 0;

        objectStack = new Stack<ThrowObject>(maxObjectCount);
        meleeEffects = new List<IEffect>();
    }

    public void AddThrowEffects(ThrowObject tobj)
    {
        if (ThrowCount == 2)
        {
            Debug.Log("<color=red>Knock back throw</color>");
            tobj.AddEffect(new KnockBack());
        }
    }

    public void AddMeleeEffect(IEffect effect)
    {
        meleeEffects.Add(effect);
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
        tobj.Throw(transform.forward + (transform.up*0.3f), throwForces[ThrowCount]);

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

        // test
        if (MeleeCount == 1)
            AddMeleeEffect(new KnockBack());

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

            damagable.TakeDamage(1);
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
