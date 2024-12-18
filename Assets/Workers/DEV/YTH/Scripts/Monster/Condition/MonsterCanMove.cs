using BehaviorDesigner.Runtime.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CondMonsterCanMove : Conditional
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _target;
    public override TaskStatus OnUpdate()
    {
        //TODO: Idle, walk ���尡 �ִٸ� �߰��� ��
      
        if (Vector3.Distance(transform.position, _target.transform.position) < _monsterData.TraceRange && !_monsterData.IsAttacked && _monsterData.Type != MonsterData.MonsterType.Range)
        {
            if (_monsterData.IsAttacked == true)
            {
                // �´� �ִϸ��̼� ���
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
