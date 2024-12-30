using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;

public class MSkill_ElectricWall : Action
{
    [Inject]
    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _player;

    private float _distance;
    
	public override void OnStart()
	{
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

	public override TaskStatus OnUpdate()
	{
        if (_distance < 40 && _monsterSkillManager.ElectricWallSkill.CanUseSkill == true && _monsterSkillManager.electricWallRoutine == null)
        {
            _monsterSkillManager.electricWallRoutine = StartCoroutine(_monsterSkillManager.ElectricWallRoutine());
            Debug.Log("일렉트릭월");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}