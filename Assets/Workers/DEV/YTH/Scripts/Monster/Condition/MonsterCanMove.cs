using BehaviorDesigner.Runtime.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CondMonsterCanMove : Conditional
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _target;
    public override TaskStatus OnUpdate()
    {
        //TODO: Idle, walk 사운드가 있다면 추가할 것
      
        if (Vector3.Distance(transform.position, _target.transform.position) < _monsterData.TraceRange && !_monsterData.IsAttacked && _monsterData.Type != MonsterData.MonsterType.Range)
        {
            if (_monsterData.IsAttacked == true)
            {
                // 맞는 애니메이션 재생
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
