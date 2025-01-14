using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_DashAttack : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private PooledObject _pooledObject;

    private PlayerController _player;

    private float _distance;

    private MonsterData _monsterData;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _pooledObject = GetComponent<PooledObject>();
        _monsterData = GetComponent<MonsterData>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if ( _distance < 30 && _monsterSkillManager.DashAttackSkill.CanUseSkill == true && _monsterSkillManager.dashAttackRoutine == null && _monsterData.CanSkill)
        {
            _pooledObject.RotateToPlayer();
            _monsterSkillManager.dashAttackRoutine = StartCoroutine(_monsterSkillManager.DashAttackRoutine());
            Debug.Log("20 DashAttackRoutine");
            SoundManager.PlaySFX(SoundManager.SoundData_M.DashAttack);
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}