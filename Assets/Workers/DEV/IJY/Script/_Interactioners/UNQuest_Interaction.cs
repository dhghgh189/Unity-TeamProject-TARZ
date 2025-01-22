using UnityEngine;
using UnityEngine.Events;

public class UNQuest_Interaction : InteractionOBJ_Base, Interaction_Ibase_Activate
{
    [SerializeField] private PlayerController player;
    private QuestManager questManager;
    public int CurCount { get { return curCount; } set { curCount = value; } }

    [Header("돌발 퀘스트 NPC")]
    [SerializeField] private int curCount;
    [SerializeField] int QuestCount;
    [SerializeField] bool isOngoing;
    [SerializeField] int Reward;

    public bool IsOngoing { get { return isOngoing; } }

    void Start() => Init();

    void Init()
    {
        player = FindObjectOfType<PlayerController>();
        questManager = FindObjectOfType<QuestManager>();
        QuestCount = Random.Range(5, 10);
        Reward = Random.Range(300, 500);
        questManager.questRewardText.text = Reward.ToString();
        curCount = 0;
        isOngoing = false;
    }

    public void Activate()
    {
        if (questManager.questPanel.activeSelf || questManager.questClearPanel.activeSelf)
        {
            Debug.Log("퀘스트 창이 이미 열려있음");
            return;
        }

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
        questManager.questPanel.SetActive(true);
        questManager.YesButton.Select();
        questManager.QuestCountText.text = $"좀비를 {QuestCount}마리 사냥하여, 수상한 자에게서 보상을 얻자!";
    }

    public void SayYes()
    {
        isOngoing = true;
        questManager.questPanel.SetActive(false);
        questManager.questDoingPanel.SetActive(true);
        questManager.questDoingText.text = $"{curCount} / <color=orange>{QuestCount}</color>";
    }

    public void SayNo()
    {
        isOngoing = false;
        questManager.questPanel.SetActive(false);
    }

    //===============================================================================

    void QuestOngoing()
    {
        if (curCount < QuestCount)
        {
            return;
        }

        ClearQuest();
    }

    void ClearQuest()
    {
        // TODO : 리워드 지급 UI

        player.Stat.BlackChip += Reward;
        isOngoing = false;

        questManager.questDoingPanel.SetActive(false);
        questManager.questClearPanel.SetActive(true);
        player.interactioner.quest_Interaction = null;
        questManager.questNPC = null;

        this.gameObject.SetActive(false);
    }

    public void OnChangeCount()
    {
        // 퀘스트가 진행중이 아니면 호출되도 아무것도 하지않음
        if (!isOngoing)
            return;

        if (curCount >= QuestCount) curCount = QuestCount;
        else curCount++;

        questManager.questDoingText.text = $"{curCount} / <color=orange>{QuestCount}</color>";
    }

    void OnDisable()
    {
        isOngoing = false;
    }
}
