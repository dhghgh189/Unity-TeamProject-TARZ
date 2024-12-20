using BehaviorDesigner.Runtime.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CondMonsterCanAttack : Conditional
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _player;

    private float _distance;

    public override TaskStatus OnUpdate()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_distance <=  _monsterData.AttackRange && !_monsterData.IsAttacked)
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
