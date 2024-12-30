using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class MonsterJustTrigger : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField] MonsterData _monsterData;

    [Inject]
    [SerializeField] StatModel _playerStat; // 젠젝

    [SerializeField] GameObject _justTrigger;

    [SerializeField] GameObject _player;

    private GameObject _returnObj;

    [Header("저스트 회피 범위")]
    [SerializeField] float _angle; // 시야각

    [SerializeField] float _distance; // 시야 거리

    private void Update()
    {
        // 저스트회피 판정 범위 내에 있으면 트리거가 켜짐
        _returnObj = WithinSight(_player, _angle, _distance);
    }

    private void Just()
    {
        if (_returnObj != null)
        {
            justRoutine = StartCoroutine(JustRoutine());
        }
    }

    WaitForSeconds delay = new(0.25f);
    Coroutine justRoutine;
    IEnumerator JustRoutine() // 켜진 트리거는 아주 짧은 시간 뒤 꺼짐
    {
        _justTrigger.SetActive(true);
        yield return delay;
        _justTrigger.SetActive(false);

        justRoutine = null;
    }

    private void OnTriggerExit(Collider other) 
    {
        // 플레이어가 대쉬기로 나갈 시에 보상을 얻음
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        if (player.Fsm.CurrentState.type == EState.Dash)
        {
            switch (_monsterData.MonsterTIer)
            {
                case MonsterData.MonsterTier.Normal:
                    // 버프 느낌
                    // 일시적 스탯 향상
                    Debug.Log("3티어 정상작동");
                    break;
                case MonsterData.MonsterTier.Elite:
                    Debug.Log("2티어 정상작동");
                    break;
                case MonsterData.MonsterTier.Boss:
                    Debug.Log("1티어 정상작동");
                    break;
            }
            Debug.Log("Just Success!");
        }
        else
        {
            Debug.Log($"Just Failed...");
        }
    }

    #region 저스트회피 범위 판정
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

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;

        // 좌측 경계 계산
        Vector3 leftBoundary = Quaternion.Euler(0, -_angle / 2, 0) * transform.forward * _distance;

        // 우측 경계 계산
        Vector3 rightBoundary = Quaternion.Euler(0, _angle / 2, 0) * transform.forward * _distance;

        // 중심에서 좌/우 경계선을 그립니다
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, position + leftBoundary);
        Gizmos.DrawLine(position, position + rightBoundary);
    }
    #endregion
}
