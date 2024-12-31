using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_Thunder : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private PooledObject _pooledObject;

    private MonsterData _monsterData;

    private PlayerController _player;

    private float _distance;

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
        if (_distance < 20 && _monsterSkillManager.ThunderSkill.CanUseSkill == true && _monsterData.CurHp <= _monsterData.MaxHp / 2 && _monsterSkillManager.thunderRoutine == null)
        {
            _monsterSkillManager.thunderRoutine = StartCoroutine(_monsterSkillManager.ThunderRoutine());
            Debug.Log("10 ThunderRoutine 시작");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
        
    }
}
