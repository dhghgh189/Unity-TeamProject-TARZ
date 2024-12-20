using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/*public interface IDamagable
{
    void TakeDamage(int damage);
}*/

public class EnemyController : MonoBehaviour, IDamagable, IKnockBack
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

    public void KnockBack(GameObject attacker)
    {
        StartCoroutine(KnockBackRoutine(attacker));
    }

    IEnumerator KnockBackRoutine(GameObject attacker)
    {
        float knockBackTime = 0.1f;
        Vector3 moveDir = (transform.position - attacker.transform.position).normalized;
        moveDir.y = 0;
        while (true)
        {
            knockBackTime -= Time.deltaTime;
            if (knockBackTime <= 0)
                yield break;

            transform.position += moveDir * 5f * Time.deltaTime; 
            yield return null;
        }
    }
}
