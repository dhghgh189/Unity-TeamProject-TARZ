using UnityEngine;

/// <summary>
/// 마나 상태
/// </summary>
public class BaseManaState : BaseState<PlayerController>, IManaAct
{

    public BaseManaState(PlayerController owner)
    {
        this.owner = owner;
    }

    public virtual void OnAction()
    {
    }

    public virtual bool OnCollisionEnterAction(Collision other)
    {
        return false;
    }

    public virtual bool OnCollisionAction(Collision other)
    {
        return false; 
    }

    public virtual bool OnCollisionExitAction(Collision other)
    {
        return false;
    }

}
