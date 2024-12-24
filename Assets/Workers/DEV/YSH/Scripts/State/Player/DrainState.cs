using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class DrainState : BaseState<PlayerController>
{ 
    public DrainState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Drain;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.SkillHandler.Use(SkillEnum.ActTimingType.Drain);

        owner.Movement.Move(Vector3.zero);
        owner.Anim.CrossFade(Define.HASH_ANIM_DRAIN, 0.125f);
        owner.Drain.StartDrain();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 드레인 중에는 스테미너 감소
        owner.Stat.ChangeStamina(-(owner.Drain.DrainStaminaAmount * Time.deltaTime));

        if (!owner.PInput.TryDrain || owner.Stat.CurrentStamina <= 0)
        {
            owner.Drain.StopDrain();
            owner.ChangeState(EState.Idle);
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
