using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTrap : Trap
{
    [SerializeField] private float damage;

    public override void Activate()
    {
        isActive = true;
    }

    public override void Deactivate()
    {
        isActive = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable == null)
                return;

            Debug.Log($"<color=red>Normal Trap 작동! : {damage}</color>");
            damagable.TakeDamage(damage);
            Deactivate();
        }
    }
}
