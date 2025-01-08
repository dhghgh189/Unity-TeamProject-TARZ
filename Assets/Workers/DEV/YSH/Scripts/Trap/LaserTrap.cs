using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : Trap
{
    [SerializeField] private float damage;
    [SerializeField] private float rayTime;     // 레이저를 발사하는 시간 (1턴)
    [SerializeField] private float interval;    // 1턴 발사 후 쉬는 시간
    [SerializeField] private Transform rayPoint1;
    [SerializeField] private Transform rayPoint2;
    [SerializeField] private LayerMask whatIsTarget;

    private Vector3 laserDir;
    private float laserDistance;
    private RaycastHit[] hits;

    private IDamagable target;

    private float timer;
    private float nextRayTime;

    public override void Activate()
    {
        isActive = true;
        nextRayTime = Time.time;
        timer = 0f;
    }

    public override void Deactivate()
    {
        isActive = false;
    }

    protected override void Init()
    {
        base.Init();
        // Ray Point1 에서 Ray Point2로 향하는 벡터
        Vector3 laserVector = rayPoint2.position - rayPoint1.position;

        // 매번 계산하지 않도록 미리 저장
        laserDir = laserVector.normalized;
        laserDistance = laserVector.magnitude;
    }

    private void Update()
    {
        if (!isActive)
            return;

        // 휴면 시간이 끝나지 않았으면 return
        if (Time.time < nextRayTime)
            return;

        // ray를 쏘는 턴 동안 타이머 동작
        timer += Time.deltaTime;
        if (timer >= rayTime)   // 턴이 종료된 경우
        {
            // 휴면 시간 저장
            nextRayTime = Time.time + interval;
            // 타이머 초기화
            timer = 0;
            return;
        }

        // 쉬는 시간도 아니고 아직 턴 중인 경우 아래의 laser 로직 실행
        Debug.DrawRay(rayPoint1.position, laserDir * laserDistance, Color.red);
        hits = Physics.RaycastAll(rayPoint1.position, laserDir, laserDistance, whatIsTarget);
        for (int i = 0; i < hits.Length; i++)
        {
            // 연속적으로 피격하는게 아닌 텀을 두고 피격하도록 구현 필요!
            // 임시 코드
            target = hits[i].collider.GetComponent<IDamagable>();
            if (target == null) 
                continue;

            target.TakeDamage(damage);
        }
    }
}
