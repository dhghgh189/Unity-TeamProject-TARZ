using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UI_Bluechip : MonoBehaviour
{
    [Inject] PlayerSkillHandler playerSkill;

    [SerializeField] private RectTransform bluechipContent;
    [SerializeField] private GameObject bluechipPrefab;

    private Dictionary<string, BluechipPanel> bluechipSkill = new();

    private void Awake()
    {
        // 블루칩 스킬 UI 업데이트 이벤트 구독
        playerSkill.onAddSkillEvents.AddListener(UpdateUI);
        Debug.Log(playerSkill.onAddSkillEvents.GetPersistentEventCount());
    }

    /// <summary>
    /// 블루칩 스킬 UI 업데이트 함수
    /// </summary>
    /// <param name="skillName"></param>
    private void UpdateUI(string skillName)
    {
        BaseSkillSO item = playerSkill.skillDic[skillName];

        if (bluechipSkill.TryGetValue(skillName, out BluechipPanel value))
        {
            value.InitBluechipSkill(item.Name, item.SkillLevel == item.MaxLevel ? "Level. Max" : $"Level. {item.SkillLevel}", item.Description, item.Icon);
            return;
        }

        BluechipPanel skill = Instantiate(bluechipPrefab, bluechipContent).GetComponent<BluechipPanel>();
        skill.InitBluechipSkill(item.Name, item.SkillLevel == item.MaxLevel ? "Level. Max" : $"Level. {item.SkillLevel}", item.Description, item.Icon);

        bluechipSkill.Add(skillName, skill);
    }
}
