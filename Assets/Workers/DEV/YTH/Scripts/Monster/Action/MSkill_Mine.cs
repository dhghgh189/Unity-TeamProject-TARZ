using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;

public class MSkill_Mine : Action
{
    [SerializeField] Transform _muzzlePoint;

    private MonsterSkillManager _monsterSkillManager;

    private PooledObject _pooledObject;

    private PlayerController _player;

    private Animator _animator;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _pooledObject = GetComponent<PooledObject>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
	{
        _player = _pooledObject.player;
        _monsterSkillManager.MuzzlePoint = _muzzlePoint;
    }

    public override TaskStatus OnUpdate()
	{
        if (_monsterSkillManager.MineSkill.CanUseSkill == true && _monsterSkillManager.mineRoutine == null)
        {
            MonsterRotation();
            Debug.Log("mine");
            _monsterSkillManager.mineRoutine = StartCoroutine(_monsterSkillManager.MineRoutine());
            _animator.SetTrigger("TakeMine");
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