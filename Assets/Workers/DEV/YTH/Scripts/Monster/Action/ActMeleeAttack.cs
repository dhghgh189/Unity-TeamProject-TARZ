using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using static BTreeState;


public class ActMeleeAttack : Action
{
    State state;

    private MonsterData _monsterData;

    private Animator _animator;

    private PlayerController _player;

    private float _distance;

    private PooledObject _pooledObject;

    public override void OnAwake()
    {
        _pooledObject = GetComponent<PooledObject>();
        _monsterData = GetComponent<MonsterData>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
    }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        //Debug.Log(state);

        if (_distance <= _monsterData.AttackRange && !_monsterData.IsCatched && attackRoutine == null)
        {
            _animator.SetBool("Move", false);
            _animator.SetTrigger("Attack");
            SoundManager.PlaySFX(SoundManager.Instance.monsterSoundDic[_monsterData.AttackID]);
            attackRoutine = StartCoroutine(AttackRoutine());

            if (animSuccessRoutine == null)
            {
                animSuccessRoutine = StartCoroutine(AnimSuccess());
            }
        }

        // 노드 진행
        switch (state)
        {
            case State.Success:
                return TaskStatus.Success;

            case State.Running:
            default:
                return TaskStatus.Running;

            case State.Failure:
                return TaskStatus.Failure;
        }
    }

    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        _pooledObject.RotateToPlayer();
        yield return new WaitForSeconds(_monsterData.AttackSpeed);
        attackRoutine = null;
    }

    Coroutine animSuccessRoutine;
    IEnumerator AnimSuccess()
    {
        state = State.Running;

        float waitTime = 0.2f;
        yield return Util.GetDelay(waitTime);
        float curAnimTime = GetCurrentAnimTime();
        yield return Util.GetDelay(curAnimTime - waitTime);

        if (_distance > _monsterData.AttackRange)
        {
            state = State.Success;
            Debug.Log("State = Suceess!!");
        }
        animSuccessRoutine = null;
    }

    public float GetCurrentAnimTime(int layer = 0)
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(layer);
        // 현재 재생되는 애니메이션의 총 길이와 speed를 계산하여 실제 재생 시간을 반환 
        return (info.length / info.speed);
    }
}