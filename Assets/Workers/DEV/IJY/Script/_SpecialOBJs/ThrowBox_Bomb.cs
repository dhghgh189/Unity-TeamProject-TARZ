using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBox_Bomb : SpecialThrowOBJ_Base
{
    private IDamagable hit;
    private LayerMask ThrowingOBJLayer;
    private LayerMask BombBoxLayer;
    private bool isThrowingOBJ = false;
    private Coroutine CheckBombRoutine;

    [Header("폭탄 상자")]
    [SerializeField] private float BombBoxDamage;
    [SerializeField] private List<GameObject> HitOBJs;


    void Start() => Init();

    void Init()
    {
        // 임시적 데미지 수치 설정
        BombBoxDamage = 20f;

        // 쓰레기 오브젝트 참조 레이어마스크
        ThrowingOBJLayer = LayerMask.NameToLayer("ThrowObject");

        // 인식하는 오브젝트 참조용 레이어 마스크
        BombBoxLayer = 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Player");
        // 이후 기타 파괴 가능한 장애물 레이어가 추가될 경우, 이어서 추가 예정
    }

    //======================================================================


    private void OnCollisionEnter(Collision collision)
    {
        // 닿은 물체가 쓰레기 오브젝트일 경우, 3초 후 폭발하도록 한다.
        if (isThrowingOBJ != true && collision.gameObject.layer == ThrowingOBJLayer)
        {
            isThrowingOBJ = true;
            if (CheckBombRoutine != null) return;
            CheckBombRoutine = StartCoroutine(BombWaitRoutine(3f));
            return;
        }
        if (isThrowing)
        {
            Bomb();
        }
    }


    /// <summary>
    /// 폭발 본기능 : 폭발 상자는 던져질 때 범위 내에 존재하는 물체들에게 데미지를 가해줄 수 있다.
    /// </summary>
    void Bomb()
    {
        Debug.Log("터졌는지 확인");
        // 범위 내 몬스터, 플레이어 등을 인식하여 리스트로 반환하는 함수 실행
        HitOBJs = CheckBombRange();

        // 각 오브젝트의 TakeDamage 함수를 호출하여 범위 데미지를 가해준다.
        foreach (GameObject obj in HitOBJs)
        {
            if (obj.TryGetComponent<IDamagable>(out IDamagable hit))
            {
                hit.TakeDamage(BombBoxDamage);
            }
            //obj.GetComponent<IDamagable>().TakeDamage(BombBoxDamage);
        }

        Destroy(this.gameObject, 0.5f);
    }


    /// <summary>
    /// 쓰레기 오브젝트와 닿았을 경우, 폭발 상자는 3초 후 폭발을 진행한다.
    /// 위 기능을 구현하기 위한 코루틴
    /// </summary>
    /// <param name="cool"></param>
    /// <returns></returns>
    IEnumerator BombWaitRoutine(float cool)
    {
        Debug.Log("루틴 실행");
        while (cool > 0.1f)
        {
            cool -= Time.deltaTime;
            yield return null;
        }

        CheckBombRoutine = null;
        Bomb();
        yield break;
    }


    /// <summary>
    /// 폭발 범위를 계산해 리스트 내에 저장하여 반환하는 함수.
    /// </summary>
    /// <returns></returns>
    List<GameObject> CheckBombRange()
    {
        List<GameObject> _targets = new List<GameObject>();
        // 해당 범위를 납작한 원으로 재구성할 필요가 있어보임.
        Collider[] collider = Physics.OverlapSphere(this.transform.position, 5f);

        foreach (Collider _col in collider)
        {
            //if (_col.gameObject.layer != BombBoxLayer) continue;
            //  임시 사용. 위 이프문 현재 동작하지 않음
            //if (_col.gameObject.layer != LayerMask.NameToLayer("Monster") && _col.gameObject.layer != LayerMask.NameToLayer("Player")) continue;
            if (((1 << _col.gameObject.layer) & BombBoxLayer.value) == 0) continue;

            Vector3 source = transform.position; source.y = 0;
            Vector3 destination = _col.transform.position; destination.y = 0;
            Vector3 targetDir = (destination - source).normalized;
            float targetAngle = Vector3.Angle(transform.forward, targetDir);

            if (targetAngle > 360f * 0.5f) continue;
            _targets.Add(_col.gameObject);
        }

        return _targets;
    }


    /// <summary>
    /// 씬 이동 시 스크립트 정리
    /// </summary>
    void OnDisable()
    {
        if (CheckBombRoutine != null)
        {
            StopCoroutine(CheckBombRoutine);
            CheckBombRoutine = null;
        }
        HitOBJs.Clear();
    }
}
