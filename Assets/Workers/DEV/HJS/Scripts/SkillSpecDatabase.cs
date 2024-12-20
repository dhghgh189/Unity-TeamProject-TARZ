using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Skill_Spec_Database")]
public class SkillSpecDatabase : ScriptableObject
{
    [Header("DataBase")]
    [SerializeField] Dictionary<string, Spec> dic;
    [Header("Skill_Spec_List")]
    [SerializeField] List<Spec> specs;

    public bool GetData(BaseSkillSO baseSkill, out Spec spec)
    {
        return dic.TryGetValue(baseSkill.Name, out spec);
    }

    private void Awake()
    {
        dic = new Dictionary<string, Spec>();
        foreach (Spec spec in specs)
        {
            Debug.LogWarning("wlSWk 테스트");
            dic.Add(spec.Name, spec);
        }
    }

    [Serializable]
    public struct Spec
    {
        [SerializeField] BaseSkillSO skill;
        [SerializeField] List<float> power;
        [SerializeField] List<float> range;
        [SerializeField] List<float> time;

        public string Name { get { Debug.Log("<color=Green>스킬을 가져오기</color>");  return skill.Name; } }

        public float Power(int level) => power[level - 1];
        public float Range(int level) => range[level - 1];
        public float Time(int level) => time[level - 1];
    }
}
