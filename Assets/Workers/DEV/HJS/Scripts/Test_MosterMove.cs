using System.Collections;
using UnityEngine;

public class Test_MosterMove : MonoBehaviour, IStatusEffect
{
    [SerializeField] float moveSpeed;
    [SerializeField] float hp;

    private void Start()
    {
        hp = 100;
        StartCoroutine(SetStartRoutine());
    }

    public void ElectroEffect(GameObject attacker, GameObject target, float dotDamage, float duration)
    {
        StartCoroutine(DotDamageRoutine(dotDamage, duration, "전기"));
    }

    public void FrozenEffect(GameObject attacker, GameObject target, float duration)
    {
        StartCoroutine(FrozenRoutine(duration));
    }

    public void SlowEffect(GameObject attacker, GameObject target, float degree, float duration)
    {
        StartCoroutine(SlowRoutine(degree, duration));
    }

    private IEnumerator DotDamageRoutine(float dotDamage, float duration, string type)
    {
        float time = 0;

        while (time < duration)
        {
            Debug.Log($"{type} 효과로 {dotDamage}의 도트데미지를 받는다!");
            hp -= dotDamage;
            yield return new WaitForSeconds(1f);
            time += 1;
        }
    }

    private IEnumerator FrozenRoutine(float duration)
    {
        Debug.Log($"빙결 효과로 {duration}동안 멈춘다!");
        float temp = moveSpeed;
        moveSpeed = 0f;
        yield return new WaitForSeconds(duration);
        moveSpeed = temp;
    }

    private IEnumerator SlowRoutine(float degree, float duration)
    {
        float temp = moveSpeed;
        moveSpeed *= degree;
        Debug.Log($"슬로우 효과로 {duration}동안 {degree}의 만큼 속도가 줄어들어 {moveSpeed}가 되었다!");
        yield return new WaitForSeconds(duration);
        moveSpeed = temp;
    }

    private IEnumerator SetStartRoutine()
    {
        yield return new WaitForSeconds(3f);
        moveSpeed = 2f;

        while (true)
        {
            yield return new WaitForSeconds(2f);
            moveSpeed *= -1;
        }
    }

    void Update()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    }

    public void PoisonEffect(GameObject attacker, GameObject target, float dotDamage, float duration)
    {
        StartCoroutine(DotDamageRoutine(dotDamage, duration, "중독"));
    }
}
