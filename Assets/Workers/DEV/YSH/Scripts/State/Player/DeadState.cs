using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : BaseState<PlayerController>
{
    float timer;

    public DeadState(PlayerController owner)
    {
        this.owner = owner;
        type = EState.Dead;
    }

    public override void OnEnter()
    {
        timer = 0;
        owner.Anim.CrossFade(Define.HASH_ANIM_DEAD, 0.125f);
        owner.StartCoroutine(GameOverRoutine());
    }

    public override void OnUpdate()
    {
        owner.Movement.Rigid.velocity = Vector3.zero;
        owner.Movement.Rigid.angularVelocity = Vector3.zero;

        //timer += Time.deltaTime;
        //if (timer >= 5f)
        //{
        //    owner.loadingObject.StartLoading(Define.SceneType.Lobby);
        //    owner.ChangeState(EState.Idle);
        //    return;
        //}
    }

    private IEnumerator GameOverRoutine()
    {
        yield return Util.GetDelay(5f);
        owner.loadingObject.StartLoading(Define.SceneType.Lobby);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
