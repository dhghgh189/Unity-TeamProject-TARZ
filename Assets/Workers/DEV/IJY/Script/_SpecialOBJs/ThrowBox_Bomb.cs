
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Zenject;

public class ThrowBox_Bomb : SpecialThrowOBJ_Base
{
    [Inject] EffectManager effectManager;
    public ThrowBox_Bomb() => box_type = Box_Type.Bomb;

    private bool isThrowingOBJ = false;
    private LayerMask ThrowingOBJLayer;
    private LayerMask BombBoxLayer;
    private Coroutine CheckBombRoutine;

    [Header("폭탄 상자")]
    [SerializeField] float range;
    [SerializeField] Vector3 Circle_R = new Vector3(5f, 0f, 5f);
    [SerializeField] private float BombBoxDamage;
    [SerializeField] private List<GameObject> HitOBJs;


    void Start() => Init();

    void Init()
    {
        // 쓰레기 오브젝트 참조 레이어마스크
        ThrowingOBJLayer = LayerMask.NameToLayer("ThrowObject");
        // 인식하는 오브젝트 참조용 레이어 마스크
        BombBoxLayer = (1 << LayerMask.NameToLayer("Monster")) | (1 << LayerMask.NameToLayer("Player"));

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
        // 플레이어가 직접적으로 던졌을 경우, 플레이어에게는 데미지를 가하지 않되, 범위 내의 객체들에게 폭발 데미지를 가한다.
        if (isThrowing)
        {
            BombBoxLayer &= ~(1 << LayerMask.NameToLayer("Player"));
            Bomb();
        }
    }


    /// <summary>
    /// 폭발 본기능 : 폭발 상자는 던져질 때 범위 내에 존재하는 물체들에게 데미지를 가해줄 수 있다.
    /// </summary>
    void Bomb()
    {
        HitOBJs = CheckBombRange();

        // 각 오브젝트의 TakeDamage 함수를 호출하여 범위 데미지를 가해준다.
        foreach (GameObject obj in HitOBJs)
        {
            if (obj.TryGetComponent<IDamagable>(out IDamagable hit))
            {
                hit.TakeDamage(BombBoxDamage);
            }
        }
        SoundManager.PlaySFX(SoundManager.SoundData_UI.Bomb);
        effectManager.ParticlePlay("FX_Explosion_01", 3f, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }


    /// <summary>
    /// 쓰레기 오브젝트와 닿았을 경우, 폭발 상자는 3초 후 폭발을 진행한다.
    /// 위 기능을 구현하기 위한 코루틴
    /// </summary>
    /// <param name="cool"></param>
    /// <returns></returns>
    IEnumerator BombWaitRoutine(float cool)
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.BombTimer);

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
        List<GameObject> list = new();
        Collider[] collider = Physics.OverlapSphere(this.transform.position, range, BombBoxLayer);
        foreach (Collider e in collider) list.Add(e.gameObject);
        return list;
    }

    public void CheckPath(GameObject Circle, Vector3 pos)
    {
        pos.y = 0.01f;
        Circle.transform.position = pos;

        if (Circle.transform.localScale != Circle_R) Circle.transform.localScale = Circle_R;
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
