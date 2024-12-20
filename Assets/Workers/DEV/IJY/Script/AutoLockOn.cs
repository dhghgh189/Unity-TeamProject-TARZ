using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoLockOn : MonoBehaviour
{
    private LayerMask monsterLayer;
    private Coroutine PlayCheck;
    private PlayerController owner;

    [Header("인식 범위")]
    [SerializeField] float range;
    [SerializeField] float angle;

    [Header("카메라")]
    [SerializeField] private CameraController cam;
    [SerializeField] private Transform defultLookAt;

    [Header("몬스터 배열")]
    [SerializeField] List<Transform> Monsters;

    // 임시적 유니티 이벤트 : 추후 몬스터 컨트롤러에 이벤트 추가 시 대체될 예정
    public event Action action;


    private void Start()
    {
        Monsters = new List<Transform>();

        defultLookAt = cam.lookAt;
        monsterLayer = LayerMask.GetMask("Monster");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            AutoLockOnStart();
        }
    }

    public void AutoLockOnStart()
    {
        // Queue 초기화 과정 진행
        if (Monsters.Count > 0)
        {
            Debug.Log("락온 배열 초기화");
            Monsters.Clear();
        }

        // TODO : 코루틴 실행 여부 체크
        if (PlayCheck == null)
        {
            // 배열에 몬스터 추가
            Monsters = CheckMonsters();
            // 코루틴 실행
            PlayCheck = StartCoroutine(LockOnRoutine());
            action += TargetingMonster;
            action.Invoke();
        }
        else if (PlayCheck != null)
        {
            Debug.Log("오토 락온 사용 중지");
            StopCoroutine(PlayCheck);

            action -= TargetingMonster;

            cam.lookAt = defultLookAt;
            PlayCheck = null;
        }
    }

    // 추후 몬스터 사망 시 마다 사용될 수 있도록, 코루틴이 아닌 이벤트 함수로 수정 예정
    IEnumerator LockOnRoutine()
    {
        // 배열에 아무런 오브젝트도 존재하고 있지 않다면 코루틴 종료
        if (IsMonsterAlive() == false)
        {
            Debug.Log("모든 몬스터 처리 완료 : 코루틴 종료");
            cam.lookAt = defultLookAt;
            PlayCheck = null;
            yield break;
        }

        cam.lookAt = Monsters[0];
        Debug.Log($"타겟팅 : {cam.lookAt}");

        yield return new WaitForSeconds(1.5f);
    }

    List<Transform> CheckMonsters()
    {
        List<Transform> targets = new List<Transform>();
        Collider[] collider = Physics.OverlapSphere(transform.position, range, monsterLayer);

        // 인식할 몬스터 각도의 범위 설정
        foreach (Collider _col in collider)
        {
            Vector3 source = transform.position; source.y = 0;
            Vector3 destination = _col.transform.position; destination.y = 0;

            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);

            if (targetAngle > angle * 0.5f) continue;

            Debug.Log("범위 내 몬스터 배열에 추가");
            targets.Add(_col.transform);
        }

        return targets;
    }
    void TargetingMonster()
    {
        Debug.Log("타겟 설정줌");

        var target = from targeting in Monsters
                     where Vector3.Distance(transform.position, Monsters[0].transform.position)
                           < Vector3.Distance(transform.position, targeting.transform.position)
                     // 플레이어와 가까운 순으로 정렬 진행
                     orderby targeting ascending
                     select targeting;

        Monsters = target.ToList();
    }
    bool IsMonsterAlive()
    {
        Debug.Log("몬스터 잔존 여부 확인");

        // bool 자료형 임시값 할당
        bool isAlive = true;

        foreach (Transform monster in Monsters)
        {
            // 배열 내에 저장된 오브젝트가 하나라도 존재할 경우, 반복문 종료 후 true 반환
            if (Monsters.Contains(monster))
            {
                isAlive = true;
                break;
            }
            // 그게 아닐 때, 즉 배열 내에 저장된 오브젝트가 없을 경우, false 반환
            else isAlive = false;
        }

        // 배열에 비활성화 상태, 혹은 missing인 몬스터가 존재할 때, 다시 배열을 정리하여 타겟을 설정한다.
        foreach (Transform t in Monsters)
        {
            if (t.gameObject != null || t.gameObject.activeSelf) continue;
            Monsters.Remove(t);
        }

        // 반복문에서 도출된 값을 반환한다.
        Debug.Log($"결과값 출력 : {isAlive}");
        return isAlive;
    }

    private void OnDrawGizmos()
    {
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
    }
}
