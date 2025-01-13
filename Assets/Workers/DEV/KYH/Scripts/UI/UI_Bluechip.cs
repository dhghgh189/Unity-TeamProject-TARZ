using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEditor.Progress;

public class UI_Bluechip : MonoBehaviour
{
    [Inject] PlayerSkillHandler playerSkill;

    [SerializeField] private RectTransform bluechipContent;
    [SerializeField] private GameObject bluechipPrefab;

    private Dictionary<string, BluechipPanel> bluechipSkill = new();

    private void Awake()
    {
        playerSkill.onAddSkillEvents.AddListener(UpdateUI);
        Debug.Log(playerSkill.onAddSkillEvents.GetPersistentEventCount());
    }

    /*private void Start()
    {
        foreach (var item in playerSkill.skillDic?.Keys)
        {
            UpdateUI(item);
        } 
    }*/

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
