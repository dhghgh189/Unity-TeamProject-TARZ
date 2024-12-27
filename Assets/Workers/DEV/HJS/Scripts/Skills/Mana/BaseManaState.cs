using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManaState : BaseState<PlayerController>, IManaAct
{

    public BaseManaState(PlayerController owner)
    {
        this.owner = owner;
    }

    public virtual void OnAction()
    {
    }

    public virtual bool OnCollisionAction(Collision other)
    {
        return false;
    }

}
