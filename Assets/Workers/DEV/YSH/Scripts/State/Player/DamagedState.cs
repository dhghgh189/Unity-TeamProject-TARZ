using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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
        owner.delay *= 0.5f;

        owner.IsAnimStart = true;
    }

    public override void OnUpdate()
    {
        owner.Movement.Rigid.angularVelocity = Vector3.zero;
        owner.Movement.Rigid.velocity = new Vector3(0, owner.Movement.Rigid.velocity.y, 0);

        if (owner.IsAnimStart == false)
        {
            owner.ChangeState(EState.Idle);
            return;
        }
    }
}
