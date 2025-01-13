using System.Collections;
using UnityEngine;
using Zenject;

public class UNQuest_Interaction : InteractionOBJ_Base, Interaction_Ibase_Activate
{
    [Inject] private PlayerController player;
    private Coroutine OngoingRoutine;

    [Header("돌발 퀘스트 NPC")]
    [SerializeField] int QuestCount;
    [SerializeField] bool isOngoing;
    [SerializeField] bool isClearQuest;
    [SerializeField] float Reward;


    void Start() => Init();

    void Init()
    {
        Reward = Random.Range(300f, 500f);
        QuestCount = 0;
        isOngoing = false;
        isClearQuest = false;
    }

    public void Activate()
    {
        // TODO : UI 대화창 띄움과 동시에, 돌발 퀘스트 승낙 여부 표시
        if (isOngoing)
        {
            QuestOngoing();
            return;
        }
        if (isClearQuest)
        {
            ClearQuest();
            return;
        }

        BeforeQuest();
    }

    //===============================================================================


    void BeforeQuest()
    {
        // TODO : UI 출력
    }

    public void SayYes()
    {
        isOngoing = true;
        OngoingRoutine = StartCoroutine(QuestOngoingRoutine(Random.Range(3, 6)));
        // TODO : UI 끔
    }

    public void SayNo()
    {
        isOngoing = false;
        // TODO : UI 끔
    }

    //===============================================================================

    void QuestOngoing()
    {
        if (OngoingRoutine != null)
        {
            // TODO : 진행하는 도중이라는 UI 표시
            // 달성률도 표시하면 좋겠다.
        }
    }

    void ClearQuest()
    {
        // TODO : 플레이어 자체적인 stat 값에 리워드를 지급한다.
        player.Stat.BlackChip += Reward;
    }

    IEnumerator QuestOngoingRoutine(int MaxCount)
    {
        // 조건 = 퀘스트 완료 조건
        while (QuestCount >= MaxCount)
        {
            //if ()
            //{
            //    QuestCount++;
            //}

            yield return null;
        }

        // 퀘스트 완료 시
        isOngoing = false;
        OngoingRoutine = null;
        isClearQuest = true;
        yield break;
    }

    void OnDisable()
    {
        isOngoing = false;
        isClearQuest = true;

        if (OngoingRoutine != null)
        {
            OngoingRoutine = null;
        }
    }
}
