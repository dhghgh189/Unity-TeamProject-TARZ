using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    // Target의 Transform
    // 플레이어가 될 수도 있고 던진 물건이 될 수 도 있음
    [SerializeField] Transform targetTrans;

    // 지나가면서 실질적으로 생기는 오브젝트
    // 이 친구가 데미지 입힌다
    [SerializeField] GameObject effect;

    public Transform SetTarget { set { targetTrans = value; } }

    // 생성을 하면서 작동을 해야하는 부분
    private void Start()
    {
        transform.forward = targetTrans.forward;

        // 시작 위치를 가지고 -> 플레이어가 시작한 부분
        float distance = effect.transform.localScale.z;

        // 시작과 동시에 바로 바닥에 장판 1개 생성
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 1f, LayerMask.GetMask("Ground", "Floor")))
        {
            if (hitInfo.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
            {
                Destroy(Instantiate(effect, hitInfo.point + Vector3.up * 0.01f + transform.forward.normalized, Quaternion.Euler(Vector3.right * 90f)), 3f);
            }
        }

        StartCoroutine(StartFloorRoutine(distance * 0.5f));
    }

    private IEnumerator StartFloorRoutine(float radius)
    {
        Vector3 spawnPos = transform.position + transform.forward.normalized * radius;

        yield return new WaitForFixedUpdate();

        Vector3 pastPos;
        Vector3 curPos;
        do
        {
            if (targetTrans is null) yield break;
            pastPos = transform.position;
            transform.position = targetTrans.position;

            yield return new WaitForFixedUpdate();
            curPos = transform.position;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 2f, LayerMask.GetMask("Ground", "Floor")))
            {
                if (hitInfo.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
                {
                    Destroy(Instantiate(effect, hitInfo.point + Vector3.up * 0.01f + transform.forward.normalized * radius, Quaternion.Euler(Vector3.right * 90f)), 3f);
                    spawnPos += transform.forward * radius;
                }
            }
        } while (targetTrans is not null && !pastPos.Equals(curPos));
        

        Debug.Log("End");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
