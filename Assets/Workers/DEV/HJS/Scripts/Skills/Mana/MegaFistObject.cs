using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 거대 주먹 오브젝트에 부착하는 스크립트
/// </summary>
public class MegaFistObject : MonoBehaviour
{
    [Header("Fist")]
    [SerializeField] GameObject fistBody;       // 주먹의 본체
    [SerializeField] BoxCollider boxCollider;   // 판정을 해주는 콜라이더
    [SerializeField] float fistSpeed;           // 주먹의 나가는 속도
    [SerializeField] float fistTime;            // 주먹이 나가는데 걸리는 시간
    [SerializeField] float length;              // 주먹의 길이
    [SerializeField] float damage;              // 주먹의 데미지  
    [Header("Materials")]                       
    [SerializeField] float flow;                // 흐름의 정도 <- 얼마만큼 흘렀는지 확인하는 척도
    [SerializeField] float curFlow;             // 현재 흐름
    [SerializeField] float alphaValue;          // 투명화 정도
    [Space(2)]
    [Header("Parts")]
    [SerializeField] List<MaterialParts> materialParts;

    public float Damage { get => damage; set { damage = value; } }
    public float AlphaValue { get => alphaValue; set { alphaValue = value; } }
    public float FistSpeed { get => fistSpeed; set { fistSpeed = value; } }
    public float FistTime { get => fistTime; set { fistTime = value; } }
    public float Length { get => length; set { length = value; } }
    public Transform FistBody { get => fistBody.transform; }

    [HideInInspector] public UnityEvent OnEndEvent;

    // 데이터 연동 + collider가 늘어나게

    [ContextMenu("data")]
    public void SetData()
    {
        foreach (var part in materialParts)
        {
            part.Setting();
        }
    }

    private void Start()
    {
        foreach (var part in materialParts)
        {
            part.SetUnvisible();
        }
    }

    public void Move()
    {
        foreach (var part in materialParts)
        {
            part.SetFlow("_AlphaValue", Mathf.Clamp(alphaValue, 0f, 1f));
            part.SetUnvisible();
        }

        fistBody.transform.localPosition = Vector3.back * fistBody.transform.localScale.z * 2f;
        flow = fistBody.transform.localScale.z;

        StartCoroutine(MoveReturnRoutine());
    }


    private IEnumerator MoveForwardRoutine(Vector3 startPos, Vector3 endPos)
    {
        yield return Util.GetDelay(1f);
        boxCollider.enabled = true;
        Debug.Log("앞으로 움직이기 시작!");
        SoundManager.PlaySFX(SoundManager.SoundData_S.ManaSkillSounds_3[1].AudioClip);

        float elapsedTime = 0f;
        while ((elapsedTime * fistSpeed) < fistTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime * fistSpeed) / fistTime);
            curFlow = Remap(Vector3.Magnitude(transform.position - startPos), 0f, length, -flow, flow);
            alphaValue = Remap(curFlow, -flow, flow, -2f, 0f);
            SetFlow(alphaValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        transform.position = endPos;

        Debug.Log("앞으로 움직이기 종료!");
    }

    private IEnumerator MoveReturnRoutine()
    {
        // 거리(최대 : Length만큼) = 속도 * 시간
        float distance = Mathf.Min(fistSpeed * fistTime, length);

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.forward * distance;

        yield return MoveForwardRoutine(startPos, endPos);
        boxCollider.enabled = false;

        yield return Util.GetDelay(1f);

        Debug.Log("뒤로 움직이기 시작!");

        float elapsedTime = 0f;
        while ((elapsedTime * fistSpeed) < fistTime)
        {
            transform.position = Vector3.Lerp(endPos, startPos, (elapsedTime * fistSpeed) / fistTime);
            curFlow = Remap(Vector3.Magnitude(transform.position - startPos), length, 0f, flow, -flow);
            alphaValue = Remap(curFlow, flow, -flow, 0f, -2f);
            SetFlow(alphaValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;

        Debug.Log("뒤로 움직이기 종료!");
        yield return Util.GetDelay(0.5f);
        OnEndEvent?.Invoke();
    }

    private void SetFlow(float value)
    {
        foreach (var part in materialParts)
        {
            part.SetFlow("_flow", value);
        }
    }

    private float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(Damage); }
        }
    }

    private void OnDestroy()
    {
        OnEndEvent.RemoveAllListeners();
        StopAllCoroutines();
    }
}
[System.Serializable]
public class MaterialParts
{
    public string indexName;
    public GameObject obj;
    public Material material;
    public float Center;
    public float Unvisible;
    public float Visible;

    private BoxCollider bc;

    public void Setting()
    {
        bc = obj.GetComponent<BoxCollider>();
        Center = bc.center.y;
        Unvisible = Center - bc.size.y * 0.5f;
        Visible = Center + bc.size.y * 0.5f;
    }

    public void SetFlow(string name, float value)
    {
        material.SetFloat(name, value);
    }

    public void SetUnvisible()
    {
        material.SetFloat("_flow", Unvisible);
    }
}

