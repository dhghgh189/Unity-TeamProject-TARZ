using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;

public class MSkill_ElectricWall : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private PooledObject _pooledObject;

    private PlayerController _player;

    private Animator _animator;

    private float _distance;

    private MonsterData _monsterData;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _pooledObject = GetComponent<PooledObject>();
        _animator = GetComponent<Animator>();
        _monsterData = GetComponent<MonsterData>();
    }

    public override void OnStart()
	{
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

	public override TaskStatus OnUpdate()
	{
        if (_distance < 40 && _monsterSkillManager.ElectricWallSkill.CanUseSkill == true && _monsterSkillManager.electricWallRoutine == null && _monsterData.CanSkill)
        {
            _pooledObject.RotateToPlayer();
            _monsterSkillManager.electricWallRoutine = StartCoroutine(_monsterSkillManager.ElectricWallRoutine());
            _animator.SetTrigger("ElectricWall");
            SoundManager.PlaySFX(SoundManager.SoundData_M.ElectricWall);
            SoundManager.PlaySFX(SoundManager.SoundData_M.ElectricWall_2);
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}