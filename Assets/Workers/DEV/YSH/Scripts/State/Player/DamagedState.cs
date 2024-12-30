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
        owner.Movement.Rigid.angularVelocity = Vector3.zero;
        owner.Movement.Rigid.velocity = Vector3.zero;

        if (owner.IsAnimStart == false)
        {
            owner.ChangeState(EState.Idle);
            return;
        }
    }
}
