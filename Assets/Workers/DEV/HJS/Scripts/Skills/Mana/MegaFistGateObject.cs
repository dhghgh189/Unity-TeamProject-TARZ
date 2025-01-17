using System.Collections;
using UnityEngine;

/// <summary>
/// 거대 주먹을 소환하는 차원문에 부착하는 스크립트
/// 주먹만 생성할 뿐 다른 역할을 하지 않는다
/// </summary>
public class MegaFistGateObject : MonoBehaviour
{
    [Header("Fist_Init")]
    [SerializeField] MegaFistObject fistPrefab;       // 거대 주먹
    [SerializeField] GameObject instance;

    private IEnumerator StartGate(MegaFistObject fist)
    {
        yield return Util.GetDelay(1f);
        fist.Move();
    }

    // 데미지, 공격 범위, 주먹 속도, 주먹 시간 , 투명도
    public void Init(ManaSkillDataSO data, float skillDamagePercent)
    {
        // 거대 주먹 생성
        instance = Instantiate(fistPrefab.gameObject);
        MegaFistObject fist = instance.GetComponent<MegaFistObject>();

        if (fist == null) return;

        /* 스팩 설정 */
        // 공격 범위
        transform.position += Vector3.up * data.GetData((int)ManaMegaFistDataType.FistHeight);
        instance.transform.position = transform.position;
        instance.transform.rotation = transform.rotation;  
        fist.FistBody.localScale = new Vector3(data.GetData((int)ManaMegaFistDataType.FistWidth), data.GetData((int)ManaMegaFistDataType.FistHeight), data.GetData((int)ManaMegaFistDataType.FistLength) * 0.25f);
        fist.Length = data.GetData((int)ManaMegaFistDataType.FistLength);
        // 주먹 속도
        fist.FistSpeed = data.GetData((int)ManaMegaFistDataType.FistSpeed);
        // 주먹 시간
        fist.FistTime = data.GetData((int)ManaMegaFistDataType.FistTime);
        // 투명도
        fist.AlphaValue = data.GetData((int)ManaMegaFistDataType.FistAlpha) * 0.01f;
        // 공격력
        fist.Damage = data.GetData((int)ManaMegaFistDataType.FistDamage) * skillDamagePercent;
        // 파괴 이벤트 설정
        fist.OnEndEvent.AddListener(IsOver);
        // 크기 설정
        fist.SetData();

        StartCoroutine(StartGate(fist));
    }

    public void IsOver() => Destroy(gameObject);

    private void OnDestroy()
    {
        SoundManager.PlaySFX(SoundManager.SoundData_S.ManaSkillSounds_3[2].AudioClip);
        Destroy(instance);
    }
}
