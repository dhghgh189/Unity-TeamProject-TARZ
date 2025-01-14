using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static BagSkillEnum;

public class DropRedChip : MonoBehaviour, Interaction_Ibase_Activate
{
    [SerializeField] BagSkillContainerSO bagSkill;
    [SerializeField] GameObject chipUI;

    private BagSkillHandler bagSkillHandler;

    private TMP_Text[] SkillInfoTexts;

    private BagSkillDataSO bagSkillSO;
    private Image skillIcon;

    Color[] skillTierColors = { Color.green, Color.green, Color.blue, Color.black };

    private void Awake()
    {
        SkillInfoTexts = GetComponentsInChildren<TMP_Text>(true);
        skillIcon = GetComponentsInChildren<Image>(true)[1];
    }

    private void Start()
    {
        DropChipInit();
        bagSkillHandler = FindAnyObjectByType<BagSkillHandler>();
    }
    public void DropChipInit()
    {
        bagSkillSO = bagSkill.GetData((BagIndexKey)Random.Range(0, (int)BagIndexKey.Length));

        SkillInfoTexts[0].text = bagSkillSO.SkillName.ToString();
        SkillInfoTexts[1].text = bagSkillSO.SkillDescription;
        skillIcon.sprite = bagSkillSO.SkillIcon;
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
        bagSkillHandler.AddBagSkill(bagSkillSO.SkillName);
        gameObject.SetActive(false);
    }
}
