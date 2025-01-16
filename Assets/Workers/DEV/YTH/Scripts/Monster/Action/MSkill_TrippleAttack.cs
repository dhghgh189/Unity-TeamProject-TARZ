using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using Zenject;
using static BTreeState;


public class MSkill_TrippleAttack : Action
{
    State state;

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
        _monsterData=GetComponent<MonsterData>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
        _distance = Vector3.Distance(transform.position, _player.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.TrippleAttackSkill.CanUseSkill == true && _distance <= 10 && _monsterSkillManager.trippleAttackRoutine == null && _monsterData.CanSkill)
        {
            _monsterSkillManager.trippleAttackRoutine = StartCoroutine(_monsterSkillManager.TrippleAttackRoutine());
            _animator.SetTrigger("TrippleAttack");
            SoundManager.PlaySFX(SoundManager.SoundData_M.TrippleAttack);

            animSuccessRoutine = StartCoroutine(AnimSuccess());
        }
        else
        {
            state = State.Failure;
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

    Coroutine animSuccessRoutine;
    IEnumerator AnimSuccess()
    {
        state = State.Running;
        yield return Util.GetDelay(4.2f);
        state = State.Success;
        Debug.Log("State = Suceess!!");
        animSuccessRoutine = null;
    }
}