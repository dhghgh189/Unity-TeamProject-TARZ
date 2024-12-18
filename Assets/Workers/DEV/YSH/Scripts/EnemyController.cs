using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage);
}

public class EnemyController : MonoBehaviour, IDamagable
{
    public Renderer render { get; private set; }
    public Color baseColor { get; private set; }

    private void Awake()
    {
        render = GetComponent<Renderer>();
        baseColor = render.material.color;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"TakeDamage : {damage}");
    }
}
