using System.Collections;
using UnityEngine;
using Zenject;

public class MonsterJustTrigger : MonoBehaviour
{
    [Inject] private StatModel _playerStat; // 젠젝

    [Header("저스트 회피 범위")]
    [SerializeField] float _angle; // 시야각

    [SerializeField] float _distance; // 시야 거리

    private Animator _animator;

    private MonsterData _monsterData;

    private GameObject _justTrigger;

    private GameObject _player;

    private PlayerAttack _playerAttack;

    [SerializeField] private float time = 0.25f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _monsterData = GetComponent<MonsterData>();

        _justTrigger = transform.Find("JustTrigger").gameObject;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerAttack = _player.GetComponent<PlayerAttack>();

    }

    private void Update()
    {
        _justTrigger.transform.localPosition = Vector3.zero;
        _justTrigger.transform.localRotation = Quaternion.identity;
    }

    private void Just()
    {
        if (IsPlayerWithinSight(_player))   // 저스트회피 판정 범위 내에 있으면 트리거가 켜짐
        {
            justRoutine = StartCoroutine(JustRoutine());
        }
    }

    Coroutine justRoutine;
    IEnumerator JustRoutine() // 켜진 트리거는 아주 짧은 시간 뒤 꺼짐
    {
        _justTrigger.SetActive(true);
        yield return Util.GetDelay(time);
        _justTrigger.SetActive(false);

        justRoutine = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_monsterData.IsDead)
            return;

        // 플레이어가 대쉬기로 나갈 시에 보상을 얻음
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        if (player.Fsm.CurrentState.type == EState.Dash)
        {
            Debug.Log("저스트회피 성공!");
            //EffectManager.instance.ParticlePlay("Nova", 2f, _player.transform.position, _player.transform.rotation);
            SoundManager.PlaySFX(SoundManager.SoundData_P.JustSuccess);

            _playerStat.CurrentMp += 10;
            _monsterData.IsCountered = true;
            _playerAttack.JustCounter(_monsterData);
        }
        else
        {
            Debug.Log($"Just Failed...");
        }
    }

    #region 저스트회피 범위 판정
    // 범위 안에 들어온 타겟을 특정해주는 함수
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
