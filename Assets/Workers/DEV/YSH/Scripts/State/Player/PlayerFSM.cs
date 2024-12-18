using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EState { Idle, Move, Dash, Jump, Fall, Throw, Melee, Drain, Length }

public class PlayerFSM
{
    private PlayerController owner;

    private BaseState<PlayerController>[] States;
    private BaseState<PlayerController> curState;

    public BaseState<PlayerController> CurrentState => curState;

    public PlayerFSM(PlayerController owner)
    {
        this.owner = owner;

        States = new BaseState<PlayerController>[(int)EState.Length];
        States[(int)EState.Idle] = new IdleState(owner);
        States[(int)EState.Move] = new MoveState(owner);
        States[(int)EState.Dash] = new DashState(owner);
        States[(int)EState.Jump] = new JumpState(owner);
        States[(int)EState.Fall] = new FallState(owner);
        States[(int)EState.Throw] = new ThrowState(owner);
        States[(int)EState.Melee] = new MeleeState(owner);
        States[(int)EState.Drain] = new DrainState(owner);

        ChangeState(EState.Idle);
    }

    public void ChangeState(EState state)
    {
        if (curState != null)
            curState.OnExit();

        curState = States[(int)state];
        curState.OnEnter();

        // test
        owner.currentStateView = curState.type;
    }

    public void OnUpdate()
    {
        if (curState == null)
            return;

        curState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        if (curState == null)
            return;

        curState.OnFixedUpdate();
    }
}
