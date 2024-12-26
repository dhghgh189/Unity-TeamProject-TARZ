using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering;

public class UseManaState : BaseState<PlayerController>
{
    public bool IsEnd{ get => owner.ManaSkillHandler.ActionEnd; }

    public UseManaState(PlayerController owner)
    {
        this.owner = owner; type = EState.ManaUse;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ManaSkillHandler.CurNode.Value.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        owner.ManaSkillHandler.CurNode.Value.OnUpdate();

        if(IsEnd)
        {
            owner.ChangeState(EState.Idle);
        }
    }

    public override void OnFixedUpdate()
    {
        owner.ManaSkillHandler.CurNode.Value.OnFixedUpdate();
    }

    public override void OnExit()
    {
        owner.ManaSkillHandler.CurNode.Value.OnExit();
        owner.ManaSkillHandler.End();
        base.OnExit();
    }
}
