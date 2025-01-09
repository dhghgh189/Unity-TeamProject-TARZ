using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitQueue : MonoBehaviour
{
    // 대기열
    private List<QueueInfo> queue = new List<QueueInfo>();
    private List<QueueInfo> removeQueue = new List<QueueInfo>();

    public bool IsTargetInQueue(GameObject target) => queue.Where(x => x.Target == target).Count() > 0;

    public void Add(GameObject target, float waitTime)
    {
        if (IsTargetInQueue(target))
        {
            Debug.Log($"{target.name} is already added!");
            return;
        }

        // target을 대기열에 추가
        QueueInfo info = new QueueInfo() { Target = target, WaitTime = waitTime, CurrentTime = 0f }; 
        queue.Add(info);

        Debug.Log($"Wait Queue 대기열 추가 : {target.name}");
    }

    public void Remove(QueueInfo info)
    {
        if (!queue.Remove(info))
        {
            Debug.Log($"Wait Queue Exception : {info.Target.name}를 대기열에서 제거 하지 못했습니다.");
            return;
        }

        Debug.Log($"Wait Queue 대기열 제거 : {info.Target.name}");
    }

    private void Update()
    {
        // 삭제할 항목에 대한 대기열 초기화
        removeQueue.Clear();

        for (int i = 0; i < queue.Count; i++)
        {
            // 대기열에 있는 항목들의 time을 진행시킨다.
            queue[i].CurrentTime += Time.deltaTime;
            // 만약 시간이 다 된 항목이 있거나 도중에 사라진 항목이 있는 경우 삭제할 항목으로 예약한다 
            // 실시간으로 지우게 되면 이후 반복문에서 카운팅이 꼬여 에러가 생길 수 있다.
            if (queue[i].CurrentTime >= queue[i].WaitTime || queue[i].Target == null)
                removeQueue.Add(queue[i]);
        }

        for (int i = 0; i < removeQueue.Count; i++)
        {
            // 예약해놓았던 항목들을 대기열에서 제거한다.
            Remove(removeQueue[i]);
        }
    }
}

// 대기열 항목 정보
public class QueueInfo
{
    public GameObject Target;
    public float WaitTime;
    public float CurrentTime;
}