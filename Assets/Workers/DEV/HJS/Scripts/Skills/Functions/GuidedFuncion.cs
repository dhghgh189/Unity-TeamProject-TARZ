using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
/// 유도 기능
/// </summary>
public class GuidedFuncion : MonoBehaviour, IEnable
{
    // 유도와 같은 적을 우선 선별 해야할 때
    [SerializeField] Transform target;
    [SerializeField] MonsterData data;
    private Collider[] colliders;
    private Rigidbody rb;

    [SerializeField] bool enable;
    [SerializeField] new string name = "GuidedFuncion";

    public bool Enable { get => enable; set => enable = value; }
    public string Name { get => name; set => name = value; }

    private Coroutine traceCoroutine;
    private Coroutine checkCoroutine;

    public void StartCheckTarget() => checkCoroutine = StartCoroutine(CheckTargetRoutine());

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = new Collider[1];
    }

    public void FixedUpdate()
    {
        // 유도하는 함수
        if (target != null && !data.IsDead)
        {
            try
            {
                // 날아가는 속도 설정
                rb.velocity = transform.forward * 3;
                // 타겟을 바라보게 회전
                Quaternion ballTargetRotation = Quaternion.LookRotation(target.position + new Vector3(0, 0.5f) - transform.position);
                // rigidbody를 움직이기
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, 180f));
            }
            finally
            {
                CheckTarget();
            }
        }
    }

    private IEnumerator CheckTargetRoutine()
    {
        while (target == null)
        {
            if (CheckTarget()) break;
            yield return null;
        }
    }

    private bool CheckTarget()
    {
        colliders[0] = null;
        // 범위 안에 있는 하나의 Monater만 overlap해서 배열에 넣기
        int nun = Physics.OverlapSphereNonAlloc(transform.position, 2f, colliders, LayerMask.GetMask("Monster"));
        Debug.Log($"상대 찾는 중... 찾은 수 {nun}"); // 디버그 용도로 숫자 받기
        // 만약 배열이 비어있지 않다면 -> 범위 안에 몬스터가 있다
        if (colliders[0] != null)
        {
            // 타겟을 설정
            data = (colliders[0].gameObject.GetComponent<MonsterData>() != null ) ?
                 colliders[0].gameObject.GetComponent<MonsterData>() : (colliders[0].gameObject.GetComponentInParent<MonsterData>());
            target = data.gameObject.transform;
            return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (checkCoroutine is not null)
        {
            StopCoroutine(checkCoroutine);
            checkCoroutine = null;
        }

        if(!other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            target = null;
        }
    }

}
