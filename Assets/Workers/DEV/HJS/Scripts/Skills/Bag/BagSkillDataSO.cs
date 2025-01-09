using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BagSkillEnum;

[CreateAssetMenu(menuName = "Scriptables/BaseBagSkillData")]
public class BagSkillDataSO : ScriptableObject
{
    [Header("스킬 데이터 정보")]
    [SerializeField] BagIndexKey skillName;
    [SerializeField, TextArea(5,3)] string skillDescription;
    [SerializeField] Sprite skillIcon;
    [SerializeField] int maxGauge;
    [SerializeField] int chargeAmount;
    [SerializeField] int useAmount;

    [Header("설정 데이터")]
    [SerializeField] List<DataInputStruct> dataInputStructs;    // 데이터 리스트

    public DataInputStruct Getdata(int index)
    {
        return dataInputStructs.Where(x => x.index == index).First();
    }

    #region 프로퍼티
    public BagIndexKey SkillName { get { return skillName; } }
    public string SkillDescription { get { return skillDescription; } }
    public int MaxGauge { get { return maxGauge; } }
    public int ChargeAmount { get { return chargeAmount; } }
    public int UseAmount { get { return useAmount; } }
    public Sprite SkillIcon { get { return skillIcon; } }
    #endregion
}
