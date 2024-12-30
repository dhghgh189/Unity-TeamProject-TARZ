using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마나 스킬의 종류
/// </summary>
public enum ManaSkillType { Skill_1, Skill_2, Skill_3, Skill_4 }
[CreateAssetMenu(menuName = "Scriptables/ManaSkillDataSO")]
public class ManaSkillDataSO : ScriptableObject
{
    [Header("Info")]
    public ManaSkillType Type;                      // 마나스킬의 종류
    public string skillName;                        // 스킬의 고유 이름
    private Dictionary<int, float> dataDictionary;  // 스킬의 필요 데이터들

    [Header("Setting")]
    [SerializeField] List<DataInputStruct> dataInputStructs;    // 데이터 리스트

    private void Awake()
    {
        dataDictionary = new Dictionary<int, float>();
        foreach (DataInputStruct data in dataInputStructs)
        {
            if (!dataDictionary.TryAdd(data.index, data.value))
            {
                Debug.Log($"{data.index}와 동일한 이름의 데이터가 있습니다!");
                break;
            }
        }
    }
    /// <summary>
    /// 원하는 데이터를 가져오는 함수
    /// </summary>
    /// <param name="index">가져올 데이터</param>
    /// <returns>해당 데이터의 값</returns>
    public float GetData(int index)
    {
        return dataDictionary[index];
    }
}

/// <summary>
/// 데이터 입력 구조체
/// </summary>
[Serializable]
public struct DataInputStruct
{
    [Header("변수명"), Space(2)]
    public string VariableName;
    [Header("값"), Space(2)]
    public float value;
    [Header("인덱스"), Space(2)]
    [Tooltip("값이 들어가기 원하는 순서")] public int index;
}

