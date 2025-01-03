using UnityEngine;

/// <summary>
/// 기존에 상태패턴에 애니메이션의 행동을 처리해줄 함수 해주는 인터페이스
/// </summary>
public interface IManaAct
{
    public void OnAction();
    public bool OnCollisionEnterAction(Collision other);
    public bool OnCollisionAction(Collision other);
    public bool OnCollisionExitAction(Collision other);
}
