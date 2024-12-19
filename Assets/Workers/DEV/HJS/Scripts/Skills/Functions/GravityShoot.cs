using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GravityShoot : MonoBehaviour
{
    [SerializeField] float operationTime;   // 동작하는 시간
    [SerializeField] float force;           // 끌어당기는 힘
    [SerializeField] float range;           // 끌어당기는 범위

    private void Start()
    {
        GetComponent<SphereCollider>().radius = range;
        Destroy(gameObject, operationTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 끌어당기기
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            Debug.Log("StartHolidng");
            StartCoroutine(StartBoilingRoutine(other.gameObject.transform));
        }
    }

    private IEnumerator StartBoilingRoutine(Transform other)
    {
        while (true) 
        {
            // 거리를 계산하고
            Vector3 relativeDirection = other.position - transform.position;
            // 거리가 중력장 크기보다 크면 중지
            if(relativeDirection.sqrMagnitude > range * range) yield break;

            // 아니면 정규화를 진행하고
            Vector3 gravityDirection = relativeDirection.normalized;

            // 현재 있는 오브젝트 방향으로 힘의 량만큼 끌어당기기
            other.gameObject.GetComponent<Rigidbody>().velocity = -gravityDirection * force;
            yield return new WaitForFixedUpdate();
        }
    }

    // 중력장 지속시간이 다 되어서 파괴되는 경우
    private void OnDestroy()
    {
        // 모든 코루틴 종료
        StopAllCoroutines();
    }
}
