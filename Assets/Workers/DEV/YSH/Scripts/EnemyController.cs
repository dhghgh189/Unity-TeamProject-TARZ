using System.Collections;
using UnityEngine;

/*public interface IDamagable
{
    void TakeDamage(int damage);
}*/

public class EnemyController : MonoBehaviour, IDamagable, IKnockBack
{
    public Renderer render { get; private set; }
    public Color baseColor { get; private set; }

    public AutoLockOn on;


    private void Awake()
    {
        // TODO : 몬스터 생성 시 AutoLockOn 젠젝트 사용해 삽입 예정
        render = GetComponent<Renderer>();
        baseColor = render.material.color;
    }

    private void OnDisable()
    {
        Debug.Log("일단 함수 실행은 됨");

        if (on.gameObject.activeSelf)
        {
            Debug.Log("이벤트 실행함");
            on.action?.Invoke();
            Debug.Log("이벤트 실행됨");
        }
        else
        {
            Debug.Log("몬스터 비활성화/삭제됨");
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"TakeDamage : {damage}");
    }

    public void KnockBack(GameObject attacker)
    {
        Debug.Log("Enemy knock back");
        StartCoroutine(KnockBackRoutine(attacker));
    }

    IEnumerator KnockBackRoutine(GameObject attacker)
    {
        float knockBackTime = 0.1f;
        Vector3 moveDir = (transform.position - attacker.transform.position).normalized;
        moveDir.y = 0;
        Debug.Log($"movedir : {moveDir}");
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
