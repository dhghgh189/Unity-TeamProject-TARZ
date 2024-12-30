using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.ReflectionBaking.Mono.Cecil;

public class MegaFistGateObject : MonoBehaviour
{
    [Header("Fist_Init")]
    [SerializeField] MegaFistObject fist;
    [SerializeField] Animator gateAnimator;
    [Header("Fist_Data")]
    [SerializeField] float fistAttackSpeed;
    [SerializeField] float fistAttackRange;
    [SerializeField] float fistReturnTime;


    private int fistStartAnimHash;
    private int fistReturnAnimHash;

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
        fist.Move();
        gateAnimator.SetFloat("FistAttackSpeed", (fistAttackSpeed > 1) ? fistAttackSpeed : 1);
        yield return Util.GetDelay(1f);
        Debug.Log("시작!");
        gateAnimator.CrossFade(fistStartAnimHash, 0.01f);
    }

    public void OnStartAction()
    {
        fist.Return();
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
        transform.localScale = new Vector3( data.GetData((int)ManaMegaFistDataType.FistRange) * 2f, data.GetData((int)ManaMegaFistDataType.FistRange) * 2f, 0.07f);
        transform.position += Vector3.up * data.GetData((int)ManaMegaFistDataType.FistRange) * 0.5f;
        // 주먹 속도
        fistAttackSpeed = data.GetData((int)ManaMegaFistDataType.FistSpeed);
        // 주먹 시간
        fistReturnTime = data.GetData((int)ManaMegaFistDataType.FistTime);
        // TODO : 투명도
    }
}
