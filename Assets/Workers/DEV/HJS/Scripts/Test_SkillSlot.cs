using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test_SkillSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Set_Skill")]
    [SerializeField] BaseSkillSO skill;
    [SerializeField] public PlayerSkillHandler playerSkillHandler;
    [Header("Inspec")]
    [SerializeField] TMP_Text skillName;
    [SerializeField] Image skillIcon;

    private void Awake()
    {
        skillName = GetComponentInChildren<TMP_Text>();
        skillIcon = GetComponentInChildren<Image>();
    }

    public void SetSlot(BaseSkillSO skill)
    {
        this.skill = skill;
        skillIcon.sprite = skill.Icon;
        skillName.text = skill.Name;
    }

    public void RemoveSlot()
    {
        skill = null;
        skillIcon.sprite = null;
        skillName.text = "Empty";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        playerSkillHandler.RemoveSkill(skill);
        RemoveSlot();
    }

    public bool Empty => skill == null;
}
