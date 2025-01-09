using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : Trap
{
    [SerializeField] private float damage;
    [SerializeField] private float rayTime;         // 레이저를 발사하는 시간 (1턴)
    [SerializeField] private float interval;        // 1턴 발사 후 쉬는 시간
    [SerializeField] private float damageWaitTime;  // 피격 후 다음 피격까지 대기할 시간 (연속 피격 방지)
    [SerializeField] private Transform rayPoint1;
    [SerializeField] private Transform rayPoint2;
    [SerializeField] private LayerMask whatIsTarget;

    [SerializeField] private Transform laserParent;

    private Vector3 laserDir;
    private float laserDistance;
    private RaycastHit[] hits;

    private IDamagable target;

    private float timer;

    private WaitQueue damagedQueue;

    public override void Activate()
    {
        isActive = true;
        timer = 0f;
        idleRoutine = null;
        laserParent.gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        isActive = false;
        laserParent.gameObject.SetActive(false);
    }

    protected override void Init()
    {
        damagedQueue = GetComponentInParent<WaitQueue>();

        // Ray Point1 에서 Ray Point2로 향하는 벡터
        Vector3 laserVector = rayPoint2.position - rayPoint1.position;

        // 매번 계산하지 않도록 미리 저장
        laserDir = laserVector.normalized;
        laserDistance = laserVector.magnitude;

        laserParent.position = rayPoint1.position;
        laserParent.localScale = new Vector3(laserParent.localScale.x, laserParent.localScale.y, laserDistance);

        base.Init();
    }

    private Coroutine idleRoutine;
    private IEnumerator IdleRoutine()
    {
        yield return Util.GetDelay(interval);
        idleRoutine = null;
        laserParent.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isActive)
            return;

        // 휴면 시간이 끝나지 않았으면 return
        if (idleRoutine != null)
            return;

        // ray를 쏘는 턴 동안 타이머 동작
        timer += Time.deltaTime;
        if (timer >= rayTime)   // 턴이 종료된 경우
        {
            // 휴면 상태
            idleRoutine = StartCoroutine(IdleRoutine());
            laserParent.gameObject.SetActive(false);
            // 타이머 초기화
            timer = 0;
            return;
        }

        // 쉬는 시간도 아니고 아직 턴 중인 경우 아래의 laser 로직 실행
        hits = Physics.RaycastAll(rayPoint1.position, laserDir, laserDistance, whatIsTarget);
        for (int i = 0; i < hits.Length; i++)
        {
            // 레이저에 감지된 오브젝트가 아직 대기열에 있는 경우 피격하지 않는다.
            if (damagedQueue.IsTargetInQueue(hits[i].collider.gameObject))
                continue;

            target = hits[i].collider.GetComponent<IDamagable>();
            if (target == null) 
                continue;

            target.TakeDamage(damage);

            // 연속 피격 당하지 않도록 waitQueue에 넣어놓는다.
            damagedQueue.Add(hits[i].collider.gameObject, damageWaitTime);
        }
    }
}
