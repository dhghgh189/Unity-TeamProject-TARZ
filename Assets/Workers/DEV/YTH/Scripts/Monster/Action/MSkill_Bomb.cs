using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zenject;

public class MSkill_Bomb : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private PooledObject _pooledObject;

    private PlayerController _player;

    [SerializeField] Transform _muzzlePoint;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _pooledObject = GetComponent<PooledObject>();
    }

    public override void OnStart()
	{
        _player = _pooledObject.player;
        _monsterSkillManager.MuzzlePoint = _muzzlePoint;
    }

    public override TaskStatus OnUpdate()
	{
        if (_monsterSkillManager.BombSkill.CanUseSkill == true && _monsterSkillManager.bombRoutine == null)
        {
            Debug.Log("Bomb");
            MonsterRotation();
            _monsterSkillManager.bombRoutine = StartCoroutine(_monsterSkillManager.BombRoutine());

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