using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CondMonsterCanMove : Conditional
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _target;

    private GameObject _returnObj;
    public GameObject ReturnObj { get { return _returnObj; } private set { } }

    [SerializeField] float _angle;

    [SerializeField] float _distance;
    public override TaskStatus OnUpdate()
    {
        _returnObj = WithinSight(_target, _angle, _distance);
        if (_returnObj != null && _monsterData.Type != MonsterData.MonsterType.Range /*||_monsterData.Attacked_First*/) // 선빵 맞은 시점에 플레이어가 시야각에 없으면 플레이어를 쳐다봐서 returnobject 위치넘겨주게 구현
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    // 범위 안에 들어온 타겟을 특정해주는 함수
    private GameObject WithinSight(GameObject target, float angleValue, float distanceValue)
    {
        if (target == null)
        {
            return null;
        }

        var direction = target.transform.position - transform.position;
        direction.y = 0;
        var angle = Vector3.Angle(direction, transform.forward);
        if (direction.magnitude < _distance && angle < _angle * 0.5f)
        {
            if (LineOfSight(target))
            {
                return target; 
            }
        }
        return null;
    }

    // 타겟이 범위 안에 들어왔는지 확인
    private bool LineOfSight(GameObject targetObject)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, targetObject.transform.position, out hit))
        {
            if (hit.transform.IsChildOf(targetObject.transform) || targetObject.transform.IsChildOf(hit.transform))
            {
                return true;
            }
        }
        return false;
    }

    // 기즈모 확인
    public override void OnDrawGizmos()
    {
        var oldColor = UnityEditor.Handles.color;
        var color = Color.yellow;
        color.a = 0.1f;
        UnityEditor.Handles.color = color;

        var halfFOV = _angle * 0.5f;
        var beginDirection = Quaternion.AngleAxis(-halfFOV, Vector3.up) * Owner.transform.forward;
        UnityEditor.Handles.DrawSolidArc(Owner.transform.position, Owner.transform.up, beginDirection, _angle, _distance);

        UnityEditor.Handles.color = oldColor;
    }
}

