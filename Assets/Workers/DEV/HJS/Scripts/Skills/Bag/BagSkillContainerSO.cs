using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BagSkillEnum;

[CreateAssetMenu(menuName = "Scriptables/BagSkillContainer")]
public class BagSkillContainerSO : ScriptableObject
{
    [SerializeField] List<BagDataStruct> datas;

    public BagSkillDataSO GetData(BagIndexKey key)
    {
        return datas.Where(x => x.key == key).First().data;
    }
}

[System.Serializable]
public struct BagDataStruct
{
    public BagIndexKey key;
    public BagSkillDataSO data;
}

