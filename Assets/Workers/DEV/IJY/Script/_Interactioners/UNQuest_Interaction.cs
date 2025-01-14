using UnityEngine;
using UnityEngine.Events;

public class UNQuest_Interaction : InteractionOBJ_Base, Interaction_Ibase_Activate
{
    [SerializeField] private PlayerController player;
    private QuestManager questManager;
    public UnityAction OnChangeQuestUI;
    public int CurCount { get { return curCount; } set { curCount = value; } }

    [Header("돌발 퀘스트 NPC")]
    [SerializeField] private int curCount;
    [SerializeField] int QuestCount;
    [SerializeField] bool isOngoing;
    [SerializeField] float Reward;


    void Start() => Init();

    void Init()
    {
        OnChangeQuestUI += OnChangeCount;

        player = FindObjectOfType<PlayerController>();
        questManager = player.gameObject.GetComponent<QuestManager>();
        QuestCount = Random.Range(3, 6);
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
        questManager.questPanel.SetActive(true);
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

        this.gameObject.SetActive(false);
    }

    private void OnChangeCount()
    {
        if (curCount >= QuestCount) curCount = QuestCount;
        else curCount++;

        questManager.questDoingText.text = $"{curCount} / <color=orange>{QuestCount}</color>";
    }

    void OnDisable()
    {
        isOngoing = false;
        OnChangeQuestUI -= OnChangeCount;
    }
}
