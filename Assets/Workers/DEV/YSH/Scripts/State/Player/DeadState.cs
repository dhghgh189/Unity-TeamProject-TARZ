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
        // 사운드 재생
        SoundManager.PlaySFX(SoundManager.SoundData_P.Dead);
        owner.Anim.CrossFade(Define.HASH_ANIM_DEAD, 0.125f);
        owner.StartCoroutine(GameOverRoutine());
        owner.saveData.chapterSaveData = new();
    }

    public override void OnUpdate()
    {
        owner.Movement.Rigid.velocity = Vector3.zero;
        owner.Movement.Rigid.angularVelocity = Vector3.zero;
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
