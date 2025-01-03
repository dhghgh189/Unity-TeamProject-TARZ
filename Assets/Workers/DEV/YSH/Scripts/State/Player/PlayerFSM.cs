using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EState { Idle, Move, Dash, Jump, JumpThrow, JumpMelee, Fall, Throw, Melee, Drain, ManaUse, Dead, Length }

public class PlayerFSM
{
    private PlayerController owner;

    private BaseState<PlayerController>[] States;
    private BaseState<PlayerController> curState;

    public BaseState<PlayerController> CurrentState => curState;

    public AblityAdapter Adapter;

    public PlayerFSM(PlayerController owner, AblityAdapter adapter)
    {
        this.owner = owner;

        States = new BaseState<PlayerController>[(int)EState.Length];
        States[(int)EState.Idle] = new IdleState(owner);
        States[(int)EState.Move] = new MoveState(owner);
        States[(int)EState.Dash] = new DashState(owner);
        States[(int)EState.Jump] = new JumpState(owner);
        States[(int)EState.JumpThrow] = new JumpThrowState(owner);
        States[(int)EState.JumpMelee] = new JumpMeleeState(owner);
        States[(int)EState.Fall] = new FallState(owner);
        States[(int)EState.Throw] = new ThrowState(owner);
        States[(int)EState.Melee] = new MeleeState(owner);
        States[(int)EState.Drain] = new DrainState(owner);
        States[(int)EState.ManaUse] = new UseManaState(owner);
        States[(int)EState.Dead] = new DeadState(owner);

        Adapter = adapter;
        ChangeState(EState.Idle);
    }

    public void ChangeState(EState state)
    {
        if (curState != null)
            curState.OnExit();

        bool isOk = Adapter.IsPlayerCollision(state);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"), isOk);

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
