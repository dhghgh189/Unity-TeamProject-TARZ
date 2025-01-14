using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BagSkillEnum;

public class BagSkill : IBagAct
{
    protected BagSkillManager manager;
    protected BagIndexKey keyName;
    protected BagSkillDataSO skilldata;
    public BagSkillDataSO SkillData { get { return skilldata; } }
    protected float maxGauge;
    public float MaxGauge { get => maxGauge; }
    protected float chargeAmount;
    protected float useAmount;
    protected float curGauge;
    protected PlayerController owner;
    protected LinkedList<BaseBagState> acts;
    public float CurGauge { get => curGauge; set => curGauge = value; }
    public BagIndexKey KeyName => keyName;
    public Sprite icon => skilldata.SkillIcon;
    public LinkedList<BaseBagState> Acts { get => acts; set { } }
    public PlayerController player
    {
        get => owner;
        set
        {
            owner = value;
            foreach (var state in acts)
            {
                state.UpdateOwner(value);
            }
        }
    }

    public BagSkillManager Manager { set => manager = value; }

    // 게이지 충전
    public void Charge() => curGauge = (curGauge + chargeAmount >= maxGauge) ? maxGauge : curGauge + chargeAmount;
    //게이지 사용 가능한지 확인
    public bool IsCanUse() => curGauge >= useAmount;
    // 게이지 사용
    public void Use() { curGauge -= useAmount; manager.OnUIUpdateEvent?.Invoke(); }
    public virtual void RetrunFeature()
    {
    }
}
