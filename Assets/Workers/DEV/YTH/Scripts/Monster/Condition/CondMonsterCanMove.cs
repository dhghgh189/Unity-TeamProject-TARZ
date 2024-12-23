using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CondMonsterCanMove : Conditional, IDamagable
{
    [SerializeField] MonsterData _monsterData;

    [SerializeField] GameObject _player; // 플레이어 위치 넘겨줄 오브젝트

    private Transform _playerFirstAttackTransform; // 플레이어 선빵 위치 받을 변수

    private GameObject _returnObj;
    public GameObject ReturnObj { get { return _returnObj; } private set { } }

    [Header("인지 범위")]
    [SerializeField] float _angle; // 시야각

    [SerializeField] float _distance; // 시야 거리

    [Header("회전")]
    [SerializeField] float _rate; // 회전 Lerp 비율

    public override TaskStatus OnUpdate()
    {
        _returnObj = WithinSight(_player, _angle, _distance);

        if (_returnObj != null && _monsterData.Type != MonsterData.MonsterType.Range)
        {

            Debug.Log("CodnMove true");
            return TaskStatus.Success;
        }
        else if (_monsterData.Attacked_First == true) // 시야각에 없어도 선빵 맞으면 데미지들어오면서 쳐다보는 로직 
        {
            TakeDamage(1/*플레이어 데미지 불러오기*/);
            return TaskStatus.Success;
        }
        else
        {
            Debug.Log("cond move false");
            return TaskStatus.Failure;
        }
    }

    #region 적 인지 로직
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
    #endregion

    public void TakeDamage(int damage)
    {
        //플레이어가 공격한 위치를 기억하고 맞으면 돌아봄
        _playerFirstAttackTransform = _player.GetComponent<Transform>();

        _monsterData.CurHp -= damage;
        _monsterData.Attacked_First = true;

        Quaternion lookRot = Quaternion.LookRotation(_playerFirstAttackTransform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, _rate * Time.deltaTime);
    }



    ///
    /// 추후 넉백 기능 추가해주세요
    ///

}

