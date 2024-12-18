using BehaviorDesigner.Runtime.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CondMonsterCanAttack : Conditional
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _target;
    public override TaskStatus OnUpdate()
    {
        
        if (Vector3.Distance(transform.position, _target.transform.position) <=  _monsterData.AttackRange && !_monsterData.IsAttacked)
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
