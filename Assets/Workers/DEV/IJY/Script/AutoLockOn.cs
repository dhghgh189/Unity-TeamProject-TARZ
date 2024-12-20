using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoLockOn : MonoBehaviour
{
    public Action action;
    private bool ToggleOn, KickDownOn;
    private LayerMask monsterLayer;
    private Coroutine PlayCheck;

    [Header("인식 범위")]
    [SerializeField] float range, angle;

    [Header("카메라")]
    [SerializeField] private CameraController cam;

    [Header("몬스터 배열")]
    [SerializeField] List<Transform> Monsters;


    private void Start()
    {
        ToggleOn = false; KickDownOn = false;
        Monsters = new List<Transform>();
        monsterLayer = LayerMask.GetMask("Monster");
    }

    private void Update()
    {
        if (!KickDownOn)
        {
            if (Input.GetKeyDown(KeyCode.RightAlt))
            {
                Toggle_AutoLockOn(ToggleOn = true);
                Debug.Log($"토글 활성화됨{ToggleOn}");
            }
            else if (Input.GetKeyUp(KeyCode.RightAlt))
            {
                Toggle_AutoLockOn(ToggleOn = false);
                Debug.Log($"토글 비활성화됨{ToggleOn}");
            }

        }
        if (!ToggleOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt)) KickDown_AutoLockOn();
        }
    }

    void Toggle_AutoLockOn(bool isPress)
    {
        Debug.Log("<color=green>오토락온 : 토글 모드</color>");
        if (Monsters.Count > 0)
        {
            Debug.Log("락온 배열 초기화");
            Monsters.Clear();
        }

        if (isPress)
        {
            action += TargetingMonster;
            action();

            ToggleOn = true;
            cam.IsAutoLockOn = true;
        }
        else if (!isPress)
        {
            if (PlayCheck != null)
            {
                StopCoroutine(PlayCheck);
                PlayCheck = null;
            }

            Debug.Log("오토 락온 사용 중지");
            action -= TargetingMonster;

            cam.target = null;
            ToggleOn = false;
            cam.IsAutoLockOn = false;
        }
    }
    void KickDown_AutoLockOn()
    {
        Debug.Log("<color=green>오토락온 : 킥다운 모드</color>");
        if (Monsters.Count > 0)
        {
            Debug.Log("락온 배열 초기화");
            Monsters.Clear();
        }

        if (PlayCheck == null)
        {
            action += TargetingMonster;
            action();
            KickDownOn = true;
            cam.IsAutoLockOn = true;
        }
        else if (PlayCheck != null)
        {
            action -= TargetingMonster;

            Debug.Log("오토 락온 사용 중지");
            StopCoroutine(PlayCheck);
            PlayCheck = null;

            cam.target = null;
            KickDownOn = false;
            cam.IsAutoLockOn = false;
        }
    }

    // 1. 몬스터 배열 삽입
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
    // 2. 배열 내 몬스터 거리순으로 정렬
    public void TargetingMonster()
    {
        // 배열 연동이 아닌 추가로 진행하자
        Monsters.AddRange(CheckMonsters());
        Monsters = Monsters.Distinct().ToList();

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
    // 3. 주기적으로 타겟팅할 몬스터를 설정
    IEnumerator LockOnRoutine()
    {
        // 배열에 아무런 오브젝트도 존재하고 있지 않다면 동작 중지
        if (IsMonsterAlive() == false)
        {
            Debug.Log("인식 가능한 몬스터 없음 : 코루틴 종료");
            action -= TargetingMonster;

            cam.target = null; PlayCheck = null;
            ToggleOn = false; KickDownOn = false;
            cam.IsAutoLockOn = false;
            yield break;
        }
        // 배열에 오브젝트가 있을 경우, 타겟 설정 진행
        cam.target = Monsters.First();
        Debug.Log($"타겟팅 : {cam.target}");

        yield return null;
    }
    // 4. 주기적으로 타겟팅을 확인할 때, 배열의 갯수 유무를 판단
    bool IsMonsterAlive()
    {
        Debug.Log("몬스터 잔존 여부 확인");
        bool isAlive = true;

        for (int i = Monsters.Count - 1; i >= 0; i--)
        {
            Debug.Log("배열 정리중");
            if (Monsters[i] == null || !Monsters[i].gameObject.activeSelf)
            {
                Debug.Log($"지워짐 : {Monsters[i]}");
                Monsters.Remove(Monsters[i]);
            }
        }
        if (Monsters.Count > 0) isAlive = true;
        else isAlive = false;

        Debug.Log($"결과값 출력 : {isAlive}");
        return isAlive;
    }
    // 5. 다시 1번으로
    // 1번 과정으로 다시 반복하는 이유 : 주변 몬스터가 추가적으로 생성되었을 가능성이 있으므로,
    // 모든 몬스터가 사라졌을 때 락온이 종료되도록 하려면 특정 주기마다 배열을 재구성 및 배열할 필요가 있다.


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
        Monsters.Clear();

        if (PlayCheck != null)
        {
            StopCoroutine(PlayCheck);
            cam.target = null; PlayCheck = null;
            ToggleOn = false; KickDownOn = false;
            cam.IsAutoLockOn = false;
        }
        action -= TargetingMonster;
    }
}
