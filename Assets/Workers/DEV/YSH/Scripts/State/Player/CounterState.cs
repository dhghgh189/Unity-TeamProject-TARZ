using System.Collections;
using UnityEngine;

public class CounterState : BaseState<PlayerController>
{
    private Transform mainCamTrf;
    private float exceptionTimer;
    public CounterState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Counter;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (mainCamTrf == null)
            mainCamTrf = Camera.main.transform;

        owner.IsImortal = true;
        exceptionTimer = 0;

        switch (owner.Attack.CounterTarget.MonsterTIer)
        {
            case MonsterData.MonsterTier.Normal:
                counterRoutine = owner.StartCoroutine(NormalCounterRoutine(owner.Attack.CounterTarget));
                break;
            case MonsterData.MonsterTier.Elite:
            case MonsterData.MonsterTier.Boss:
                counterRoutine = owner.StartCoroutine(BossCounterRoutine(owner.Attack.CounterTarget));
                break;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        exceptionTimer += Time.deltaTime;

        // 예외처리
        if (exceptionTimer >= 5f)
        {
            Debug.LogWarning("<color=red>CounterState Exception!!</color>");
            owner.ChangeState(EState.Idle);
            return;
        }

        // 일반 몬스터 반격 시 카메라 회전하면 캐릭터도 같이 회전
        if (owner.Attack.CounterTarget.MonsterTIer == MonsterData.MonsterTier.Normal)
            owner.transform.forward = mainCamTrf.forward;

        if (counterRoutine == null)
        {
            Debug.Log("플레이어의 반격 정상 종료");
            owner.ChangeState(EState.Idle);
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.IsImortal = false;
        owner.Attack.CounterTarget = null;
    }

    Coroutine counterRoutine;
    IEnumerator NormalCounterRoutine(MonsterData monster)
    {
        // 카메라 방향 쳐다보기
        owner.transform.forward = mainCamTrf.forward;
        // 몬스터 콜라이더 끄기
        monster.coll.enabled = false;
        // 몬스터 집어들기
        Grab(monster);

        // 던지기 전 잠시 대기
        yield return Util.GetDelay(0.5f);

        // 애니메이션
        owner.Anim.CrossFade(Define.HASH_ANIM_COUNTER_THROW, 0.01f);

        yield return Util.GetDelay(0.1f);
        yield return Util.GetDelay(owner.GetCurrentAnimTime(1));

        // 카운터 끝
        counterRoutine = null;
    }

    private void Grab(MonsterData monster)
    {
        // 충돌 끄기
        Physics.IgnoreCollision(owner.coll, monster.coll, true);

        monster.IsCatched = true;
        monster.agent.enabled = false;

        // 충돌한 몬스터 손에 잡기
        monster.transform.parent = owner.GrabPoint;
        monster.transform.localPosition = Vector3.zero;
        monster.transform.localRotation = Quaternion.identity;
    }

    IEnumerator BossCounterRoutine(MonsterData monster)
    {
        Vector3 monsterPos;

        while (true)
        {
            monsterPos = monster.transform.position - owner.transform.position;
            if (monsterPos.sqrMagnitude < owner.Attack.CounterMeleeRange)
            {
                break;
            }
            else
            {
                owner.Movement.Rigid.velocity = monsterPos.normalized * owner.Stat.MoveSpeed;
                owner.transform.forward = monsterPos.normalized;
            }
            yield return null;
        }

        SoundManager.PlaySFX(SoundManager.SoundData_P.EliteCounter);

        //근접 공격
        owner.Anim.CrossFade(Define.HASH_ANIM_COUNTER_MELEE, 0.01f);

        yield return Util.GetDelay(0.1f);
        owner.Movement.Rigid.AddForce((-owner.transform.forward + Vector3.up * 0.5f) * 12f, ForceMode.Impulse);
        yield return Util.GetDelay(owner.GetCurrentAnimTime());

        // 카운터 끝
        counterRoutine = null;
    }
}
