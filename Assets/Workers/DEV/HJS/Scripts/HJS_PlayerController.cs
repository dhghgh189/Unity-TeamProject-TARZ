using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActTiming = SkillEnum.ActTimingType;

public class HJS_PlayerController : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] PlayerSkillHandler skillHandler;
    [SerializeField] ActTiming timing;
    [Header("Attack")]
    [SerializeField] GameObject throwPrefab;
    [SerializeField] Transform muzzlePoint;
    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [Space(5)]
    [Header("Inspec")]
    [SerializeField] Rigidbody rb;
    private Coroutine coroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 스킬 사용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skillHandler.Use(timing);
            if (timing.Equals(ActTiming.Attack))
            {
                GameObject instance = Instantiate(throwPrefab, muzzlePoint.position, muzzlePoint.rotation);
                instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 2 + Vector3.forward * 3, ForceMode.Impulse);
                instance.GetComponent<Test_ThrowObject>().handler = skillHandler;
            }
            else if (timing.Equals(ActTiming.Dash) && coroutine is null)
            {
                coroutine = StartCoroutine(MoveRoutine());
            }
        }
    }

    private IEnumerator MoveRoutine()
    {
        float elapsedTime = 0;
        Vector3 direction = (transform.forward * dashSpeed);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + direction;

        while (elapsedTime < direction.magnitude / dashSpeed)
        {
            rb.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / (direction.magnitude / dashSpeed));

            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        coroutine = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌 발생!");
        skillHandler.PlayerCollision(timing, collision.gameObject);
        if (timing.Equals(ActTiming.Dash) && coroutine is not null)
        {
            Debug.Log("Stop");
            StopCoroutine(coroutine);
            coroutine = null;
        }
        // 실제론 이렇게 들어감
        // skillHandler.PlayerCollision(ActTiming.Attack, collision.gameObject)
    }

}
