using BehaviorDesigner.Runtime.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class CondCanAttack : Conditional
{
    private MonsterData _monsterData;

    [Inject] private PlayerController _player;

    private float _distance;

    public override void OnAwake()
    {
        _monsterData = GetComponent<MonsterData>();
    }

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_distance <=  _monsterData.AttackRange /*&& !_monsterData.IsAttacked*/)
        {
            Debug.Log("CondMonsterCanAttack!!!!!!");
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
