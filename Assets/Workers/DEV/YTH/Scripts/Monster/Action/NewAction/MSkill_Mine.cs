using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;

public class MSkill_Mine : Action
{
    [Inject]
    private CoroutineManager _util;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _player;

    [SerializeField] Transform _muzzlePoint;
    
	public override void OnStart()
	{
        _monsterSkillManager.MuzzlePoint = _muzzlePoint;
    }

    public override TaskStatus OnUpdate()
	{
        if (_monsterSkillManager.MineSkill.CanUseSkill == true)
        {
            Debug.Log("Bomb");
            MonsterRotation();
            _util.StartRoutine(ref _monsterSkillManager.mineRoutine, _monsterSkillManager.MineRoutine());
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    public void MonsterRotation()
    {
        transform.LookAt(_player.transform);
    }
}