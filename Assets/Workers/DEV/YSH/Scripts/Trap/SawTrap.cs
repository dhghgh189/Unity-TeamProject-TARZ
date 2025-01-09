using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : Trap
{
    [SerializeField] private Transform rotator;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float damageWaitTime;

    private WaitQueue damagedQueue;
    private CollisionHandler[] colls;

    public override void Activate()
    {
        isActive = true;
    }

    public override void Deactivate()
    {
        isActive = false;
    }

    protected override void Init()
    {
        damagedQueue = GetComponentInParent<WaitQueue>();
        colls = GetComponentsInChildren<CollisionHandler>();
        base.Init();
    }

    private void Start()
    {
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].Coll.isTrigger)
                colls[i].OnTriggerStayed += CheckTrigger;
            else
                colls[i].OnCollisionStayed += CheckCollision;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].Coll.isTrigger)
                colls[i].OnTriggerStayed -= CheckTrigger;
            else
                colls[i].OnCollisionStayed -= CheckCollision;
        }
    }

    private void Update()
    {
        if (!isActive)
            return;

        rotator.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    public void CheckTrigger(Collider other)
    {
        if (!isActive)
            return;

        if (damagedQueue.IsTargetInQueue(other.gameObject))
            return;

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")
            && other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable == null)
            return;

        damagable.TakeDamage(damage);
        damagedQueue.Add(other.gameObject, damageWaitTime);
    }

    public void CheckCollision(Collision other)
    {
        if (!isActive)
            return;

        if (damagedQueue.IsTargetInQueue(other.gameObject))
            return;

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")
            && other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable == null)
            return;

        damagable.TakeDamage(damage);
        damagedQueue.Add(other.gameObject, damageWaitTime);
    }
}
