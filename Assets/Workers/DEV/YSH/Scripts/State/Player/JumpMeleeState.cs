using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMeleeState : BaseState<PlayerController>
{
    public JumpMeleeState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.JumpMelee;
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
