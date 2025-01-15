using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// 장판을 생성해주는 스크립트
/// </summary>
public class FloorSpawner : MonoBehaviour, ISpec
{
    // Target의 Transform
    // 플레이어가 될 수도 있고 던진 물건이 될 수 도 있음
    [SerializeField] Transform targetTrans;
    [SerializeField] SkillSoundScript script;

    [SerializeField] SkillEnum.InteractionType type;
    [SerializeField] FloorArea area;
    [SerializeField] FloorObject effect;             // 지나가면서 실질적으로 생기는 오브젝트
    [SerializeField] float time = 0.35f;            // 지속시간 시간, 대쉬일 때
    // 스펙에 필요한 정보들
    private Spec spec;
    private int level;
    private FloorObject floorInstance;
    private FloorArea areaInstance;
    public Transform SetTarget { set { targetTrans = value; } }

    private void Awake()
    {
        script = GetComponent<SkillSoundScript>();
    }

    // 생성을 하면서 작동을 해야하는 부분
    private void Start()
    {
        // 생성했을 때 앞을 바라보게
        transform.rotation = targetTrans.rotation;

        areaInstance = Instantiate(area);
        areaInstance.Type = type;
        areaInstance.Init(spec, level);

        // 시작 위치를 가지고 -> 플레이어가 시작한 부분
        float distance = effect.gameObject.transform.localScale.z;

        // 시작과 동시에 바로 바닥에 장판 1개 생성
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Ground")))
        {
            floorInstance = Instantiate(effect, hitInfo.point, transform.rotation);
            // 이친구의 부모를 Area로 
            floorInstance.transform.parent = areaInstance.transform;
        }

        StartCoroutine(StartFloorRoutine(distance * 0.5f));
    }

    // 종료하는 시점은 Target이 파괴되거나 특정 시간이 다 되었을 때 파괴
    private IEnumerator StartFloorRoutine(float radius)
    {
        Vector3 spawnPos = transform.position + transform.forward.normalized * radius;
        float curtime = 0f;
        yield return new WaitForFixedUpdate();
        // 효과음 
        script.PlaySound();

        Vector3 pastPos;
        Vector3 curPos;
        do
        {
            if (targetTrans is null || curtime >= time) break;
            
            pastPos = transform.position;
            transform.position = targetTrans.position;

            yield return new WaitForFixedUpdate();
            curPos = transform.position;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Ground")))
            {
                floorInstance = Instantiate(effect, hitInfo.point, transform.rotation);
                floorInstance.transform.parent = areaInstance.transform;
                spawnPos += transform.forward * radius;
            }

            curtime += Time.deltaTime;
        } while (targetTrans is not null && !pastPos.Equals(curPos));

        Debug.Log("End");
        Destroy(gameObject);
    }

    public void SetSpec(Spec spec, int level)
    {
        this.spec = spec;
        this.level = level;
        Debug.Log("Init!");
    }
}
