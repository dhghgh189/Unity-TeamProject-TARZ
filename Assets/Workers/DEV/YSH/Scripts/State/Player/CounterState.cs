using System.Collections;
using UnityEngine;

public class CounterState : BaseState<PlayerController>
{
    private Transform mainCamTrf;
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
        if (counterRoutine == null)
        {
            owner.ChangeState(EState.Idle);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        owner.IsImortal = false;
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

        // 애니메이션 (추후 추가 필요)
        //owner.Anim.CrossFade($"Melee1", 0.01f);

        //yield return Util.GetDelay(0.1f);
        //yield return Util.GetDelay(owner.GetCurrentAnimTime());

        // 던지기 전 잠시 대기
        yield return Util.GetDelay(0.5f);

        // 던지기
        Throw(monster);

        yield return Util.GetDelay(0.5f);
        monster.coll.enabled = true;

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

    private void Throw(MonsterData monster)
    {
        monster.transform.parent = null;
        monster.transform.position = owner.GrabPoint.position;
        monster.transform.rotation = Quaternion.identity;
        monster.rigid.constraints = RigidbodyConstraints.None;
        monster.rigid.AddForce((mainCamTrf.forward + Vector3.up) * owner.Attack.CounterThrowForce, ForceMode.Impulse);
        monster.rigid.AddTorque(mainCamTrf.right * 3f, ForceMode.Impulse);
    }

    IEnumerator BossCounterRoutine(MonsterData monster)
    {
        Vector3 monsterPos;

        while (true)
        {
            monsterPos = monster.transform.position - owner.transform.position;
            if (monsterPos.sqrMagnitude < owner.Attack.CounterRange)
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

        //근접 공격
        owner.Anim.CrossFade($"Melee1", 0.01f);

        yield return Util.GetDelay(0.1f);
        yield return Util.GetDelay(owner.GetCurrentAnimTime());

        // 카운터 끝
        counterRoutine = null;
    }
}
