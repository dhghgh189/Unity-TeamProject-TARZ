using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropBlueChip : MonoBehaviour, Interaction_Ibase_Activate
{
    [SerializeField] SkillSpecDatabase skillSpecDatabase;
    [SerializeField] GameObject chipUI;

    private PlayerSkillHandler playerSkillHandler;

    private TMP_Text[] SkillInfoTexts;

    private BaseSkillSO baseSkillSO;
    private Image skillIcon;

    Color[] skillTierColors = { Color.green, Color.green, Color.blue, Color.black };

    private void Awake()
    {
        SkillInfoTexts = GetComponentsInChildren<TMP_Text>(true);
        skillIcon = GetComponentsInChildren<Image>(true)[1];
    }

    private void Start()
    {
        DropChipInit(1);
        playerSkillHandler = FindAnyObjectByType<PlayerSkillHandler>();
    }
    public void DropChipInit(int tier)
    {
        baseSkillSO = skillSpecDatabase.GetSkill(tier);

        SkillInfoTexts[0].text = baseSkillSO.Name;
        SkillInfoTexts[0].color = skillTierColors[baseSkillSO.SkillTier];

        SkillInfoTexts[1].text = baseSkillSO.Description;
        SkillInfoTexts[2].text = $"{1}";
        skillIcon.sprite = baseSkillSO.Icon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player")) 
            return;
        chipUI.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.CompareTag("Player"))
            return;
        chipUI.SetActive(false);
    }

    public void Activate()
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.GetBlueChip);
        playerSkillHandler.AddSkill(baseSkillSO.Name);
        gameObject.SetActive(false);
    }
}
