using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static BTreeState;


public class MSkill_FrogJump : Action
{
    BTreeState.State state;

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

        _pooledObject.RotateToPlayer();

        Debug.Log("스킬 점프 : 4");
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.frogJumpAttackRoutine == null && _distance <= _monsterData.CanJumpDistance)
        {
            _animator.SetBool("Move", false);
            _animator.SetTrigger("Jump");
            _monsterSkillManager.frogJumpAttackRoutine = StartCoroutine(_monsterSkillManager.FrogJumpAttackRoutine());

            if (animSuccessRoutine == null)
            {
                animSuccessRoutine = StartCoroutine(AnimSuccess());
            }
        }
       /* else if (_distance <= _monsterData.AttackRange)
        {
            state = BTreeState.State.Success;
        }*/
        

        // 노드 진행
        switch (state)
        {
            case BTreeState.State.Success:
                return TaskStatus.Success;

            case BTreeState.State.Running:
            default:
                return TaskStatus.Running;

            case BTreeState.State.Failure:
                return TaskStatus.Failure;
        }
    }

    Coroutine animSuccessRoutine;
    IEnumerator AnimSuccess()
    {
        state = BTreeState.State.Running;

        float waitTime = 0.2f;
        yield return Util.GetDelay(waitTime);
        float curAnimTime = GetCurrentAnimTime();
        yield return Util.GetDelay(curAnimTime - waitTime);

        state = BTreeState.State.Success;

        animSuccessRoutine = null;
    }

    public float GetCurrentAnimTime(int layer = 0)
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(layer);
        // 현재 재생되는 애니메이션의 총 길이와 speed를 계산하여 실제 재생 시간을 반환 
        return (info.length / info.speed);
    }
}
