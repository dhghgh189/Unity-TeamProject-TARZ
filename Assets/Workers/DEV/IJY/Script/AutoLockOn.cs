using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoLockOn : MonoBehaviour
{
    // 현재 락온이 가능하나, 카메라가 lookat의 position을 따르고 있어 캐릭터 이동과 카메라가 따로 노는 상태
    // 필요한 것
    // 1. 카메라는 마우스 회전을 받지 않을 것 (=화면고정)
    // 2. 카메라는 여전히 플레이어의 뒤에 있을 것
    // lookat의 대체가 아닌 다른 무언가가 필요해보임  (lookat을 통해 카메라의 위치가 정해져서 X)

    private LayerMask monsterLayer;
    private Coroutine PlayCheck;
    private Transform defultLookAt;

    [Header("인식 범위")]
    [SerializeField] float range;
    [SerializeField] float angle;

    [Header("카메라")]
    [SerializeField] private CameraController cam;

    [Header("몬스터 배열")]
    [SerializeField] List<Transform> Monsters;

    public Action action;


    private void Start()
    {
        Monsters = new List<Transform>();
        monsterLayer = LayerMask.GetMask("Monster");

        defultLookAt = cam.lookAt;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            AutoLockOnStart();
    }

    public void AutoLockOnStart()
    {
        if (Monsters.Count > 0)
        {
            Debug.Log("락온 배열 초기화");
            Monsters.Clear();
        }

        if (PlayCheck == null)
        {
            action += TargetingMonster;
            action();
        }
        else if (PlayCheck != null)
        {
            action -= TargetingMonster;

            Debug.Log("오토 락온 사용 중지");
            StopCoroutine(PlayCheck);

            cam.lookAt = defultLookAt;
            PlayCheck = null;
        }
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
    public void TargetingMonster()
    {
        Debug.Log("타겟 설정중");
        Monsters = CheckMonsters();

        foreach (Transform col in Monsters)
        {
            Debug.Log($"배열값 : {col}");
        }

        var target = from targeting in Monsters
                     orderby Vector3.Distance(targeting.position, transform.position) ascending
                     select targeting;

        Monsters = target.ToList();

        foreach (Transform col in Monsters)
        {
            Debug.Log($"<color=yellow>재설정 배열값 : {col}</color>");
        }

        PlayCheck = StartCoroutine(LockOnRoutine());
    }
    bool IsMonsterAlive()
    {
        Debug.Log("몬스터 잔존 여부 확인");
        bool isAlive = true;

        if (Monsters.Count > 0) isAlive = true;
        else isAlive = false;

        // 배열에 비활성화 상태, 혹은 missing인 몬스터가 존재할 때, 다시 배열을 정리하여 타겟을 설정한다.
        foreach (Transform t in Monsters)
        {
            if (t.gameObject == null || t.gameObject.activeSelf == false)
                Monsters.Remove(t);
            else break;
        }

        Debug.Log($"결과값 출력 : {isAlive}");
        return isAlive;
    }
    IEnumerator LockOnRoutine()
    {
        // 배열에 아무런 오브젝트도 존재하고 있지 않다면 동작 중지
        if (IsMonsterAlive() == false)
        {
            Debug.Log("인식 가능한 몬스터 없음 : 코루틴 종료");
            action -= TargetingMonster;

            cam.lookAt = defultLookAt;
            PlayCheck = null;
            yield break;
        }

        // 배열에 오브젝트가 있을 경우, 타겟 설정 진행
        cam.lookAt = Monsters[0];
        Debug.Log($"타겟팅 : {cam.lookAt}");

        yield return null;
    }

    private void OnDrawGizmos()
    {
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
    }

    private void OnDisable()
    {
        Debug.Log("몬스터 배열 잇음");
        Monsters.Clear();
        Debug.Log("몬스터 배열 없음");

        Debug.Log("락온 배열 초기화");
        StopCoroutine(PlayCheck);
        action -= TargetingMonster;
    }
}
