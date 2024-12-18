using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        owner.Movement.Move(Vector3.zero);
        // 임시 작성, drain 애니메이션으로 바꿔야 함
        owner.Anim.CrossFade(Define.HASH_ANIM_IDLE, 0.125f);
        owner.Drain.StartDrain();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!owner.PInput.TryDrain)
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
