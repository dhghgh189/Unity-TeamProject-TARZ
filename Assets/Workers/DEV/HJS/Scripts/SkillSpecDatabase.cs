using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬이 들어있는 데이터베이스 -> 블루칩들
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Skill_Database")]
public class SkillSpecDatabase : ScriptableObject
{
    [Header("Init")]
    [SerializeField, Min(1), Tooltip("보여줄 스킬의 수")] int showCount;
    [SerializeField, Min(1), Tooltip("나올 수 있는 최대 레벨")] int maxLevel;
    private BaseSkillSO[] showSkillArray;

    [Header("Skills")]
    [SerializeField] List<BaseSkillSO> skillList;

    private void Awake()
    {
        showSkillArray = new BaseSkillSO[showCount];
    }

    /// <summary>
    /// 보여줄 스킬의 수만큼 담아서 보내준다.
    /// </summary>
    public BaseSkillSO[] ShowSkillArray()
    {
        for (int i = 0; i < showCount; i++)
        {
            showSkillArray[i] = skillList[Random.Range(0, skillList.Count)];
        }

        return showSkillArray;
    }

    /// <summary>
    /// 새로 고침을 한 스킬
    /// </summary>
    /// <returns>새롭게 나온 스킬</returns>
    public BaseSkillSO RerollSkill()
    {
        return skillList[Random.Range(0, skillList.Count)];
    }
}