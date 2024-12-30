using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaFistObject : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] BoxCollider coll;

    public float Damage { get => damage; set { damage = value; } }

    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
    }

    public void Move() =>  coll.enabled = true;

    public void Return() => coll.enabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(Damage); }
        }
    }

}
