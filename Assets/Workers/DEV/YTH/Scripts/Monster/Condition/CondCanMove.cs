using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CondCanMove : Conditional
{
    [Header("인지 범위")]
    [SerializeField] float _angle; // 시야각

    [SerializeField] float _distance; // 시야 거리

    [Header("회전")]
    [SerializeField] float _rate; // 회전 Lerp 비율

    private MonsterData _monsterData;

    private PlayerController _player; // 플레이어 위치 넘겨줄 오브젝트

    private PooledObject _pooledObject;

    public override void OnAwake()
    {
        _monsterData = GetComponent<MonsterData>();
        _pooledObject = GetComponent<PooledObject>();
    }

    public override void OnStart()
    {
        _player = _pooledObject.player;
    }

    public override TaskStatus OnUpdate()
    {
        if (IsPlayerWithinSight(_player.gameObject) && !_monsterData.IsAttacked && !_monsterData.IsCatched && _monsterData.IsDead == false)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    #region 적 인지 로직
    public bool IsPlayerWithinSight(GameObject target)
    {
        if (target == null)
            return false;

        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0;

        if (direction.magnitude < _distance && Vector3.Angle(direction, transform.forward) < _angle * 0.5f)
        {
            if (Physics.Linecast(transform.position, target.transform.position, out RaycastHit hit))
            {
                if (hit.transform.IsChildOf(target.transform) || target.transform.IsChildOf(hit.transform))
                {
                    return true;
                }
            }
        }
        return false;
    }

#if UNITY_EDITOR
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
#endif
    #endregion
}

