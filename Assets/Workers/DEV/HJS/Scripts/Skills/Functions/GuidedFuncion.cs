using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유도 기능
/// </summary>
public class GuidedFuncion : MonoBehaviour
{
    // 유도와 같은 적을 우선 선별 해야할 때
    [SerializeField] Transform target;
    private Collider[] colliders;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = new Collider[1];
    }

    // 일단 테스트 용으로 FixedUpdate에 구현 -> 코루틴에 구현 예정
    public void FixedUpdate()
    {
        // 유도하는 함수
        if (target != null)
        {
            // 날아가는 속도 설정
            rb.velocity = transform.forward * 3;
            // 타겟을 바라보게 회전
            Quaternion ballTargetRotation = Quaternion.LookRotation(target.position + new Vector3(0, 0.8f) - transform.position);
            // rigidbody를 움직이기
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, 180f));
        }
    }
    private void Update()
    {
        // 이게 타겟 설정
        if (target == null)
        {
            // 범위 안에 있는 하나의 Monater만 overlap해서 배열에 넣기
            int nun = Physics.OverlapSphereNonAlloc(transform.position, 3f, colliders, LayerMask.GetMask("Monster"));
            Debug.Log(nun); // 디버그 용도로 숫자 받기
            // 만약 배열이 비어있지 않다면 -> 범위 안에 몬스터가 있다
            if (colliders[0] is not null)
            {
                // 타겟을 설정
                target = colliders[0].gameObject.transform;
            }
        }
    }
    private void Start()
    {
        // 여기서 해도 된다
        // 여기서 하면 coroutine 으로 작동하는 형식으로
        // 코루틴에 적이 있는지 확인하고 
        // 있으면 코루틴을 멈추기 Update에서 추적 or 코루틴에서 추적
        // StartCoroutine(TraceRoutine());
    }

    private IEnumerator TraceRoutine()
    {
        yield return new WaitUntil(() => { return target != null; });

        //rb.position = Vector3.MoveTowards(rb.position, target.position, 5f * Time.fixedDeltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target.position, 1f);
    }

    private void OnDrawGizmos()
    {
        if(enabled == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 3f);
        }
    }

}
