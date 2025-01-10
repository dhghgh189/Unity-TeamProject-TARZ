using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrapCore : MonoBehaviour, IDamagable
{
    [SerializeField] private int health;

    public void SetCoreHealth(int helath)
    {
        this.health = helath;
    }

    public void TakeDamage(float damage)
    {
        health--;
        if (health <= 0)
        {
            Debug.Log("Slow Trap 파괴");
            Destroy(transform.parent.gameObject);
        }
    }
}
