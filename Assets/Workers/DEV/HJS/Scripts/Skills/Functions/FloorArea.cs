using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloorArea : MonoBehaviour
{
    private List<GameObject> lists;
    private Interaction interaction;
    private SkillEnum.InteractionType type;
    private WaitQueue interactionQueue;
    public SkillEnum.InteractionType Type { set { type = value; } }

    private void Awake()
    {
        interactionQueue = GetComponent<WaitQueue>();
    }

    // 필요한 데이터
    // 데미지, 지속시간
    // 이펙트 작동
    public void Init(Spec spec, int level)
    {
        // 값을 설정해주고
        interaction = new Interaction(type);
        interaction.SetSpec(spec, level);
    }

    private void Start()
    {
        lists = new List<GameObject>();
        StartCoroutine(CheckChildRoutine());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            if (interactionQueue.IsTargetInQueue(other.gameObject)) return;

            interaction.Activate(gameObject, other.gameObject);

            interactionQueue.Add(other.gameObject, 1f);
        }
    }

    private IEnumerator CheckChildRoutine()
    {
        // 바로 확인하면 생성되기전에 삭제가 되므로 임시적으로 대쉬의 시간만큼 딜레이
        float curTime = 0f;
        while (curTime < 0.35) 
        { 
            curTime += Time.deltaTime;
            yield return null;
        }

        while(true)
        {
            if (transform.childCount <= 0) break;
            yield return null;
        }
        yield return Util.GetDelay(1f);
        Debug.Log("Floor Area 삭제!");
        Destroy(gameObject);
    }
}
