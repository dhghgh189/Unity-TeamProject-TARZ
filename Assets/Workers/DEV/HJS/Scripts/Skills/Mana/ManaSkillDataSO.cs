using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum ManaSkillType { Skill_1, Skill_2, Skill_3, Skill_4 }
[CreateAssetMenu(menuName = "Scriptables/ManaSkillDataSO")]
public class ManaSkillDataSO : ScriptableObject
{
    [Header("Info")]
    public ManaSkillType Type;
    public string skillName;
    private Dictionary<int, (string, float)> dataDictionary;

    [Header("Setting")]
    [SerializeField] List<DataInputStruct> dataInputStructs;

    private void Awake()
    {
        dataDictionary = new Dictionary<int, (string, float)>();
        foreach(DataInputStruct data in dataInputStructs)
        {
            if (!dataDictionary.TryAdd(data.index, (data.VariableName, data.value)))
            {
                Debug.Log($"{data.index}와 동일한 이름의 데이터가 있습니다!");
                break;
            }
        }
    }

    public float GetData(int index)
    {
        return dataDictionary[index].Item2;
    }
}

[Serializable]
public struct DataInputStruct
{
    [Header("변수명"),Space(2)]
    public string VariableName;
    [Header("값"), Space(2)]
    public float value;
    [Header("인덱스"), Space(2)]
    [Tooltip("값이 들어가기 원하는 순서")]public int index;
}

