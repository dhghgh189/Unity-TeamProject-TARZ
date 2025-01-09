using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MSkill_FrogJump : Action
{
    [SerializeField] ActMove_Block _actMoveBlock;

    private MonsterSkillManager _monsterSkillManager;

    private Animator _animator;

    private MonsterData _monsterData;

    private PooledObject _pooledObject;

    private PlayerController _player;

    private float _distance;

    public override void OnAwake()
    {
        _monsterSkillManager = GetComponent<MonsterSkillManager>();
        _animator = GetComponent<Animator>();
        _monsterData = GetComponent<MonsterData>();
        _pooledObject = GetComponent<PooledObject>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;

        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.frogJumpAttackRoutine == null && _distance <= _monsterData.CanJumpDistance)
        {
            _pooledObject.RotateToPlayer();
            _monsterSkillManager.frogJumpAttackRoutine = StartCoroutine(_monsterSkillManager.FrogJumpAttackRoutine());
            _animator.SetTrigger("Jump");
            Debug.Log("개구리 점프!!");
            return TaskStatus.Success;
        }
        else if (_distance <= _monsterData.AttackRange)
        {
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
