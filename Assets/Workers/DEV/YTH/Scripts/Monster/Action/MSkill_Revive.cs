using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_Revive : Action
{
    [SerializeField] GameObject _reviveBefore;

    [SerializeField] GameObject _reviveAfter;

    private MonsterSkillManager _monsterSkillManager;

    private Animator _animator;

    private MonsterData _monsterData;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _animator = GetComponent<Animator>();
        _monsterData = GetComponent<MonsterData>();
    }

    public override void OnStart()
    {
        _monsterSkillManager.ReviveAfter = _reviveAfter;
        _monsterSkillManager.ReviveBefore = _reviveBefore;
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterData.CurHp <= _monsterData.MaxHp / 2 && _monsterSkillManager.ReviveSkill.CanUseSkill)
        {
            _monsterSkillManager.Revive();
            _animator.SetBool("Move", false);   
            _animator.SetBool("Revive", true);
            Debug.Log("부활");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
