using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class QuestManager : MonoBehaviour
{
    [Inject] ChangeInput input;

    public UNQuest_Interaction questNPC;
    [SerializeField] public GameObject questPanel;
    [SerializeField] public TMP_Text QuestCountText;
    [SerializeField] public GameObject questDoingPanel;
    [SerializeField] public TMP_Text questDoingText;
    [SerializeField] public GameObject questClearPanel;
    [SerializeField] public TMP_Text questRewardText;
    [SerializeField] private Button yesButton;
    public Button YesButton { get { return yesButton; } }

    public void OnClickYes()
    {
        questNPC.SayYes();
    }

    public void OnClickNo()
    {
        questNPC.SayNo();
    }
}