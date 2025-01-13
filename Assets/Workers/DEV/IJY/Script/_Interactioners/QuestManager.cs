using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private UNQuest_Interaction questNPC;
    [SerializeField] public GameObject questPanel;
    [SerializeField] public TMP_Text QuestCountText;

    private void Start()
    {
        questNPC = GetComponent<UNQuest_Interaction>();
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