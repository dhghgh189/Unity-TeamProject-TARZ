using UnityEngine;
using Zenject;

public class UNQuest_Interaction : InteractionOBJ_Base, Interaction_Ibase_Activate
{
    [Inject] private PlayerController player;
    public int CurCount { get { return curCount; } set { curCount = value; } }

    private QuestManager questManager;

    [Header("돌발 퀘스트 NPC")]
    [SerializeField] private int curCount;
    [SerializeField] int QuestCount;
    [SerializeField] bool isOngoing;
    [SerializeField] float Reward;


    void Start() => Init();

    void Init()
    {
        questManager = player.gameObject.GetComponent<QuestManager>();

        Reward = Random.Range(300f, 500f);
        curCount = 0;
        isOngoing = false;
    }

    public void Activate()
    {
        if (isOngoing)
        {
            QuestOngoing();
            return;
        }

        BeforeQuest();
    }

    //===============================================================================

    void BeforeQuest()
    {
        // TODO : UI 출력
        // 플레이어 움직임 정지
        questManager.questPanel.SetActive(true);
        questManager.QuestCountText.text = $"좀비를 {QuestCount}마리 사냥하여, 수상한 자에게서 보상을 얻자!";
    }

    public void SayYes()
    {
        isOngoing = true;
        curCount = 0;
        QuestCount = Random.Range(3, 6);
        // TODO : UI 끔
        questManager.questPanel.SetActive(false);
        // 퀘스트 UI도 구성하면 좋겠다
    }

    public void SayNo()
    {
        isOngoing = false;
        curCount = 0;
        // TODO : UI 끔
        questManager.questPanel.SetActive(false);
    }

    //===============================================================================

    void QuestOngoing()
    {
        if (curCount < QuestCount)
        {
            // TODO : 진행하는 도중이라는 UI 표시
            // 달성률도 표시하면 좋겠다.
            questManager.questDoingPanel.SetActive(true);
            questManager.questDoingText.text = $"{curCount} / <color=orange>{QuestCount}</color>";
            return;
        }

        ClearQuest();
    }

    void ClearQuest()
    {
        player.Stat.BlackChip += Reward;
        isOngoing = false;
        questManager.questDoingPanel.SetActive(false);
    }

    void OnDisable()
    {
        isOngoing = false;
    }
}
