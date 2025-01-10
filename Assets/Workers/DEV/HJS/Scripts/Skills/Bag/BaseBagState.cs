using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBagState : BaseState<PlayerController>
{
    public BaseBagState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void UpdateOwner(PlayerController owner) => this.owner = owner;

    public virtual void OnAction()
    {
    }

}
