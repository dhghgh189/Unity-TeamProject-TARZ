using UnityEngine;

public class DamagedState : BaseState<PlayerController>
{
    public DamagedState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Damaged;
    }

    public override void OnEnter()
    {
        owner.Anim.CrossFade(Define.HASH_ANIM_DAMAGED, 0.1f);
        owner.delay = owner.GetCurrentAnimTime();
        owner.IsAnimStart = true;
    }

    public override void OnUpdate()
    {
        if (owner.IsAnimStart == false)
        {
            owner.ChangeState(EState.Idle);
            return;
        }
    }
}
