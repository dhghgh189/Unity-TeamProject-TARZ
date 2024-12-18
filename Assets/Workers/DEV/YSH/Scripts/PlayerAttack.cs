using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // ���⼭ throw�� melee ���¸� �����ϰ�, ���ؼ� ȥ�� �޺� ������ �����Ѱ�?

    [SerializeField] private Transform throwPoint;
    [SerializeField] private ThrowObject throwObject; // �׽�Ʈ��, �� ������Ʈ������ �÷��̾��� stack���� ����������
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float[] throwForces;
    [SerializeField] private float[] meleeAngles;
    [SerializeField] private float[] meleeRanges;

    Vector3 source;
    Vector3 dest;
    float resultAngle;

    public LayerMask WhatIsEnemy { get { return whatIsEnemy; } }

    public int MeleeCount { get; set; }
    public int ThrowCount { get; set; }
    public int ThrowCountMax => throwForces.Length;
    public int MeleeCountMax => meleeAngles.Length;

    private void Awake()
    {
        MeleeCount = 0;
        ThrowCount = 0;
    }

    public void Throw()
    {
        Debug.Log($"ThrowCount : {ThrowCount}");
        ThrowObject tobj = Instantiate(throwObject, throwPoint.position, Quaternion.identity);
        tobj.IsCollected = true;
        tobj.Throw(transform.forward + (transform.up*0.3f), throwForces[ThrowCount]);

        // ���� �� ���� ī�޶� ������ �ٶ󺸵��� �ϴ� ���� �߰� �ʿ� 

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

        // ���� �� ���� ī�޶� ������ �ٶ󺸵��� �ϴ� ���� �߰� �ʿ� 

        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRanges[MeleeCount], whatIsEnemy);
        foreach (Collider col in colliders)
        {
            source = transform.position;
            dest = col.transform.position;
            source.y = 0;
            dest.y = 0;

            resultAngle = Vector3.Angle(transform.forward, (dest - source).normalized);
            if (resultAngle > meleeAngles[MeleeCount] * 0.5f)
                continue;

            IDamagable damagable = col.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(1);
            }
        }

        if (MeleeCount < MeleeCountMax - 1)
            MeleeCount++;
        else
            MeleeCount = 0;
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
