using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class CounterState : BaseState<PlayerController>
{
    public CounterState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Counter;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.IsImortal = true;

        switch (owner.Attack.CounterTarget.MonsterTIer)
        {
            case MonsterData.MonsterTier.Normal:
                // TODO : 일반 반격
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
