/// <summary>
/// 플레이어의 마나 상태
/// </summary>
public class UseManaState : BaseState<PlayerController>
{
    // 해당 능력의 사용이  끝났는지 확인하는 변수
    public bool IsEnd { get => owner.ManaSkillHandler.ActionEnd; }

    public UseManaState(PlayerController owner)
    {
        this.owner = owner; type = EState.ManaUse;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        owner.ManaSkillHandler.CurNode.Value.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (owner.PInput.TryDash && owner.IsEnoughStamina(owner.Stat.DashStaminaAmount))
        {
            owner.ChangeState(EState.Dash);
            return;
        }

        owner.ManaSkillHandler.CurNode.Value.OnUpdate();

        if (IsEnd)
        {
            owner.ChangeState(EState.Idle);
        }
    }

    public override void OnFixedUpdate()
    {
        owner.ManaSkillHandler.CurNode.Value.OnFixedUpdate();
    }

    public override void OnExit()
    {
        owner.ManaSkillHandler.CurNode.Value.OnExit();
        owner.ManaSkillHandler.End();
        base.OnExit();
    }
}
