using System.Collections;
using UnityEngine;

/// <summary>
/// 거대 주먹을 소환하는 차원문에 부착하는 스크립트
/// </summary>
public class MegaFistGateObject : MonoBehaviour
{
    [Header("Fist_Init")]
    [SerializeField] MegaFistObject fist;       // 거대 주먹
    [SerializeField] Animator gateAnimator;     // 게이트의 애니메이터
    [Header("Fist_Data")]
    [SerializeField] float fistAttackSpeed;     // 주먹이 움직이는 속도
    [SerializeField] float fistAttackRange;     // 주먹의 공격 범위
    [SerializeField] float fistReturnTime;      // 주먹이 차원문으로 돌아오는데 걸리는 시간


    private int fistStartAnimHash;              // 주먹이 나가는 애니메이션의 해쉬
    private int fistReturnAnimHash;             // 주먹이 돌아가는 애니메이션의 해쉬

    private void Awake()
    {
        gateAnimator = GetComponent<Animator>();
        fistStartAnimHash = Animator.StringToHash("FistStartAnimation");
        fistReturnAnimHash = Animator.StringToHash("FistReturnAnimation");
    }

    private void Start()
    {
        StartCoroutine(FistStartRoutine());
    }

    // 시작할 때 애니메이션 실행
    private IEnumerator FistStartRoutine()
    {
        // 충돌체를 키고
        fist.Move();
        // 날아가는 속도를 설정한 다음
        gateAnimator.SetFloat("FistAttackSpeed", (fistAttackSpeed > 1) ? fistAttackSpeed : 1);
        yield return Util.GetDelay(1f);
        Debug.Log("시작!");
        gateAnimator.CrossFade(fistStartAnimHash, 0.01f);
    }

    /// <summary>
    /// 주먹이 다 나갔으면
    /// </summary>
    public void OnStartAction()
    {
        // 충돌체를 끄고
        fist.Return();
        // 다시 돌아오기
        StartCoroutine(FistReturnRotine());
    }

    private IEnumerator FistReturnRotine()
    {
        yield return Util.GetDelay(fistReturnTime);
        gateAnimator.CrossFade(fistReturnAnimHash, 0.01f);

    }

    public void OnEndAction()
    {
        Destroy(gameObject);
    }

    // 데미지, 공격 범위, 주먹 속도, 주먹 시간 , 투명도
    public void Init(ManaSkillDataSO data)
    {
        // 데미지
        fist.Damage = data.GetData((int)ManaMegaFistDataType.FistDamage);
        // 공격 범위
        transform.localScale = new Vector3(data.GetData((int)ManaMegaFistDataType.FistRange) * 2f, data.GetData((int)ManaMegaFistDataType.FistRange) * 2f, 0.07f);
        transform.position += Vector3.up * data.GetData((int)ManaMegaFistDataType.FistRange) * 0.5f;
        // 주먹 속도
        fistAttackSpeed = data.GetData((int)ManaMegaFistDataType.FistSpeed);
        // 주먹 시간
        fistReturnTime = data.GetData((int)ManaMegaFistDataType.FistTime);
        // TODO : 투명도
    }
}
