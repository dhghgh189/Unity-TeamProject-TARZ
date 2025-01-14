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
    [SerializeField] private Button yesButton;

    private void Start()
    {
        if (questPanel.activeSelf)
        {
            input.firstInput = yesButton;
            input.firstInput.Select();
        }
    }

    public void OnClickYes()
    {
        questNPC.SayYes();
    }

    public void OnClickNo()
    {
        questNPC.SayNo();
    }
}