using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Zenject;

public class MSkill_WheelWind : Action
{
    private MonsterSkillManager _monsterSkillManager;

    private Animator _animator;

    private PooledObject _pooledObject;

    private MonsterData _monsterData;

    [SerializeField] GameObject _wheelWindTrigger;

    private PlayerController _player;

    private float _distance;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _pooledObject = GetComponent<PooledObject>();
        _monsterData = GetComponent<MonsterData>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        _monsterSkillManager.WheelWindTrigger = _wheelWindTrigger;
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.WheelWindSkill.CanUseSkill == true && _monsterData.CurHp <= _monsterData.MaxHp / 2 && _distance <= 10 && _monsterSkillManager.wheelWindRoutine == null)
        {
            _monsterSkillManager.wheelWindRoutine = StartCoroutine(_monsterSkillManager.WheelWindRoutine());
            _animator.SetTrigger("WheelWind");
            Debug.Log("wheelWind");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}


