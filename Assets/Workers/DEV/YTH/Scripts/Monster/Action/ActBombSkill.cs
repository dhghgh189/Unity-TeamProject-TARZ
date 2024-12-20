using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ActBombSkill : Action
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] MonsterSkillManager _monsterSkillManager;

    [SerializeField] GameObject _player;

    [SerializeField] NavMeshAgent _agent;

    public override TaskStatus OnUpdate()
    {
        if (_monsterSkillManager.BombSkill.CanUseSkill == true)
        {
            MonsterRotation();
            if (_monsterSkillManager.bombRoutine == null)
            {
                _monsterSkillManager.bombRoutine =  StartCoroutine(_monsterSkillManager.BombRoutine());
                Debug.Log("Bomb");
            }
            return TaskStatus.Success;
        }
        else if (_monsterSkillManager.MineSkill.CanUseSkill == true)
        {
            MonsterRotation();
            if (_monsterSkillManager.mineRoutine == null)
            {
                _monsterSkillManager.mineRoutine = StartCoroutine(_monsterSkillManager.MineRoutine());
                Debug.Log("mine");
            }
            return TaskStatus.Success;
        }
        else
        {
            return  TaskStatus.Failure;
        }
    }

    public void OnDisable()
    {
        _monsterSkillManager.StimPak();
    }

    public void MonsterRotation()
    {
        transform.LookAt(_player.transform);
    }
}
