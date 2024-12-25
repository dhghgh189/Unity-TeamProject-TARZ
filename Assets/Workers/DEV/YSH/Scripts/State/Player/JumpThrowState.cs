using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThrowState : BaseState<PlayerController>
{
    public JumpThrowState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.JumpThrow;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
