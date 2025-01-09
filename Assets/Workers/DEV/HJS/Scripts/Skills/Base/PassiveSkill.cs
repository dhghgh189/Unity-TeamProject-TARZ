using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using static SkillEnum;

[Serializable]
public class PassiveSkill
{
    [SerializeField] string indexName;
    private StatModel statModel;
    private SkillSpecDatabase skillSpecDatabase;
    private int level;
    private BaseSkillSO parent;
    private Dictionary<ConditionType, Func<bool>> conditions;

    [Space(2)]
    [SerializeField] PassiveType passiveType;
    [Space(2)]
    [Header("Settings")]
    [Space(3)]
    [SerializeField] ModifySetting modifySetting;
    [Space(3)]
    [SerializeField] ConditionSetting conditionSetting;
    [Space(3)]
    [SerializeField] ToggleSetting toggleSetting;

    #region 패시브 프로퍼티
    public StatModel StatModel { set { statModel = value; Debug.Log("<color=yellow>패시브 스킬 StatModel 설정</color>"); } }
    public SkillSpecDatabase SkillSpecDatabase { set { skillSpecDatabase = value; Debug.Log("<color=yellow>패시브 스킬 스펙SO 설정</color>"); } }
    public BaseSkillSO Parent { set { parent = value; Debug.Log("<color=yellow>패시브 스킬부모 설정</color>"); level = parent.SkillLevel; } }
    public ModifySetting GetModifySetting => modifySetting;
    public ConditionSetting GetConditionSetting => conditionSetting;
    public ToggleSetting GetToggleSetting => toggleSetting;
    public PassiveType GetPassiveType => passiveType;
    #endregion

    #region Passive - Modify(값 설정 로직)
    // Modify - 수정
    // 값의 수정을 담당
    // 해당 적용을 할 때 바로 state에게 적용할 거 같다
    // 대폭 이렇게 있지만 -> 공격력을 더해준다
    [Serializable]
    public class ModifySetting
    {
        // Stat의 값 조절
        public PassiveModifyType ModifyType;
        public PassiveModifyInputType inputType;
        [Tooltip("ex) 100% = 100, 50% = 50")] [SerializeField] Vector3 amount;
        public float Amount(int value) => amount[value - 1];
        [HideInInspector] public float TempValue;
    }

    /// <summary>
    /// 값을 변경해줄 함수
    /// </summary>
    public void SetValue()
    {
        float tempValue = 0f;
        
        switch (GetModifySetting.ModifyType)
        {
            case PassiveModifyType.MaxHp:
                tempValue = statModel.MaxHp;
                if (GetModifySetting.inputType == PassiveModifyInputType.Value)
                {
                    statModel.MaxHp = GetModifySetting.Amount(level);
                    statModel.CurrentHp = statModel.CurrentHp;
                }
                else if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.SetAbility(AdditionAbility.MaxHpPer, GetModifySetting.Amount(level));
                }
                break;
            case PassiveModifyType.MaxStamina:
                tempValue = statModel.MaxStamina;
                if (GetModifySetting.inputType == PassiveModifyInputType.Value)
                {
                    statModel.MaxStamina = GetModifySetting.Amount(level);
                    statModel.CurrentStamina = statModel.CurrentStamina;
                }
                else if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.SetAbility(AdditionAbility.MaxStaminaPer, GetModifySetting.Amount(level));
                }
                break;
            case PassiveModifyType.CurStamina:
                if (GetModifySetting.inputType == PassiveModifyInputType.Value)
                {
                    statModel.CurrentStamina = GetModifySetting.Amount(level);
                }
                else if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.CurrentStamina = statModel.CurrentStamina * (GetModifySetting.Amount(level) * 0.01f);
                }
                break;
            case PassiveModifyType.MoveSpeed:
                tempValue = statModel.MoveSpeed;
                if (GetModifySetting.inputType == PassiveModifyInputType.Value)
                {
                    statModel.MoveSpeed = GetModifySetting.Amount(level);
                    Debug.Log("TestOn");
                }
                else if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.SetAbility(AdditionAbility.MoveSpeedPer, GetModifySetting.Amount(level));
                }
                break;
            case PassiveModifyType.AllPower:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.SetAbility(AdditionAbility.AllPowerPer, GetModifySetting.Amount(level));
                }
                else
                {
                    Debug.LogWarning("<Color=red>추가 능력치는 수치를 값으로 설정할 수 없습니다.</color>");
                }
                break;
            case PassiveModifyType.DefaultPower:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.SetAbility(AdditionAbility.DefaultPowerPer, GetModifySetting.Amount(level));
                }
                else Debug.LogWarning("<Color=red>추가 능력치는 수치를 값으로 설정할 수 없습니다.</color>");
                break;
            case PassiveModifyType.StaminaCostRate:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.StaminaCostRate = Mathf.Clamp((GetModifySetting.Amount(level) * 0.01f), 0f, 1f);
                }
                else Debug.LogWarning("<Color=red>추가 능력치는 수치를 값으로 설정할 수 없습니다.</color>");
                break;
            case PassiveModifyType.StaminaChargeRate:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                {
                    statModel.StaminaChargeRate = Mathf.Clamp((GetModifySetting.Amount(level) * 0.01f), 0f, 1f);
                }
                else Debug.LogWarning("<Color=red>추가 능력치는 수치를 값으로 설정할 수 없습니다.</color>");
                break;
        }
        GetModifySetting.TempValue = tempValue;
    }
    /// <summary>
    /// 변경한 값을 원복해주는 함수
    /// </summary>
    public void ResetValue(int setLevel = -1)
    {
        int level = (setLevel == -1) ? this.level : setLevel;
        switch (GetModifySetting.ModifyType)
        {
            case PassiveModifyType.MaxHp:
                if (GetModifySetting.inputType == PassiveModifyInputType.Value) statModel.MaxHp = GetModifySetting.TempValue;
                else if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.MaxHpPer, -(GetModifySetting.Amount(level)));
                break;
            case PassiveModifyType.MaxStamina:
                if (GetModifySetting.inputType == PassiveModifyInputType.Value) statModel.MaxStamina = GetModifySetting.TempValue;
                else if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.MaxStaminaPer, -(GetModifySetting.Amount(level)));
                break;
            case PassiveModifyType.MoveSpeed:
                if (GetModifySetting.inputType == PassiveModifyInputType.Value) statModel.MoveSpeed = GetModifySetting.TempValue;
                else if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.MoveSpeedPer, -(GetModifySetting.Amount(level)));
                break;
            case PassiveModifyType.AllPower:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.AllPowerPer, -(GetModifySetting.Amount(level)));
                break;
            case PassiveModifyType.DefaultPower:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.DefaultPowerPer, -(GetModifySetting.Amount(level)));
                break;
            case PassiveModifyType.StaminaCostRate:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.StaminaCostRate += Mathf.Clamp((1 - GetModifySetting.Amount(level) * 0.01f), 0f, 1f);
                break;
            case PassiveModifyType.StaminaChargeRate:
                if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.StaminaChargeRate += Mathf.Clamp((1 - GetModifySetting.Amount(level) * 0.01f), 0f, 1f);
                break;
        }
    }
    #endregion

    #region Passive - Condition(조건에 따른 행동 로직)
    // Condition - 조건
    // 값의 변경에 따라 행동 담당
    // 해당 내용은 MVC 에서 -> Model의 이벤트에 연결해서 사용할 거 같다.
    // 함수의 내용은 따로 정의 해야할 거 같다
    [Serializable]
    public class ConditionSetting
    {
        // 비교할 특성 -> model의 이벤트에 연결을 위한 Key 역할
        public PassiveModifyType modifyType;
        // 부등호
        public ConditionType Condition;
        // 비교할 값
        public float CompareValue;
        public void SetMax(float value) => MaxValue = value;
        [HideInInspector] public float MaxValue;
        [Space(2)]
        [Header("Result")]
        [HideInInspector] public bool isChanged;
        public PassiveResultModifyType resultModifyType;
        public float Amount(int value) => amount[value - 1];
        [SerializeField] Vector3 amount;
    }

    /// <summary>
    /// 이벤트에 부착되어서 조건을 확인할 함수
    /// </summary>
    /// <param name="data">조건의 대상</param>
    public void ConditionCheck(float data)
    {
        // 비율 계산
        float value = (conditionSetting.CompareValue * 0.01f) * conditionSetting.MaxValue;
        Debug.Log($"{parent.Name} 스킬의 조건 : {GetConditionSetting.modifyType}의 값이 {value} 보다(와) {conditionSetting.Condition} 이다 / {conditionSetting.MaxValue}, {conditionSetting.CompareValue}");

        conditions = new Dictionary<ConditionType, Func<bool>>()
            {
                { ConditionType.GreaterEqual, () => data >= value},
                { ConditionType.Greater, () => data > value},
                { ConditionType.Equal, () => data.Equals(value)},
                { ConditionType.LessEqual, () => data <= value},
                { ConditionType.Less, () => data < value},
                { ConditionType.NotEqual, () => !data.Equals(value)},
            };

        float amount = conditionSetting.Amount(level);

        if (conditions.TryGetValue(conditionSetting.Condition, out Func<bool> conditionsFunc))
        {
            if (conditionsFunc())
            {
                if (conditionSetting.isChanged) return;

                conditionSetting.isChanged = true;

                switch (conditionSetting.resultModifyType)
                {
                    case PassiveResultModifyType.MaxHp: Debug.Log("HP+"); statModel.SetAbility(AdditionAbility.MaxHpPer, (amount)); break;
                    case PassiveResultModifyType.MaxStamina: Debug.Log("ST+"); statModel.SetAbility(AdditionAbility.MaxStaminaPer, (amount)); break;
                    case PassiveResultModifyType.AllPower: Debug.Log("AP+"); statModel.SetAbility(AdditionAbility.AllPowerPer, (amount)); break;
                    case PassiveResultModifyType.DefaultPower: Debug.Log("DP+"); statModel.SetAbility(AdditionAbility.DefaultPowerPer, (amount)); break;
                    case PassiveResultModifyType.StaminaCostRate: Debug.Log("SC+"); statModel.StaminaCostRate = Mathf.Clamp(amount * 0.01f, 0f, 1f); break;
                    case PassiveResultModifyType.ExtraDamage: Debug.Log("ED+"); statModel.ExtraDamage += amount; break;
                }
            }
            else if (conditionSetting.isChanged)
            {
                conditionSetting.isChanged = false;
                switch (conditionSetting.resultModifyType)
                {
                    case PassiveResultModifyType.MaxHp: Debug.Log("HP-"); statModel.SetAbility(AdditionAbility.MaxHpPer, (-amount)); break;
                    case PassiveResultModifyType.MaxStamina: Debug.Log("ST-"); statModel.SetAbility(AdditionAbility.MaxStaminaPer, (-amount)); break;
                    case PassiveResultModifyType.AllPower: Debug.Log("AP-"); statModel.SetAbility(AdditionAbility.AllPowerPer, (-amount)); break;
                    case PassiveResultModifyType.DefaultPower: Debug.Log("DP-"); statModel.SetAbility(AdditionAbility.DefaultPowerPer, (-amount)); break;
                    case PassiveResultModifyType.StaminaCostRate: Debug.Log("SC-"); statModel.StaminaCostRate += Mathf.Clamp(1 - amount * 0.01f, 0f, 1f); break;
                    case PassiveResultModifyType.ExtraDamage: Debug.Log("ED-"); statModel.ExtraDamage -= amount; break;
                }
            }
        }
        else
        {
            Debug.LogError($"{parent.Name}의 passive 스킬의 범위가 없습니다!");
        }
    }

    /// <summary>
    /// 값을 원복시켜주는 함수
    /// </summary>
    public void ReturnValue(int level = -1)
    {
        float amount = conditionSetting.Amount((level == -1) ?  this.level: level);
        if (conditionSetting.isChanged)
        {
            conditionSetting.isChanged = false;
            switch (conditionSetting.resultModifyType)
            {
                case PassiveResultModifyType.MaxHp: Debug.Log("HP-"); statModel.SetAbility(AdditionAbility.MaxHpPer, (-amount)); break;
                case PassiveResultModifyType.MaxStamina: Debug.Log("ST-"); statModel.SetAbility(AdditionAbility.MaxStaminaPer, (-amount)); break;
                case PassiveResultModifyType.AllPower: Debug.Log("AP-"); statModel.SetAbility(AdditionAbility.AllPowerPer, (-amount)); break;
                case PassiveResultModifyType.DefaultPower: Debug.Log("DP-"); statModel.SetAbility(AdditionAbility.DefaultPowerPer, (-amount)); break;
                case PassiveResultModifyType.StaminaCostRate: Debug.Log("SC-"); statModel.StaminaCostRate = Mathf.Lerp(0f, 1f, 1 - amount * 0.01f); break;
                case PassiveResultModifyType.ExtraDamage: Debug.Log("ED-"); statModel.ExtraDamage -= amount; break;
            }
        }
    }
    #endregion

    #region Passive - Toggle(특정 기능 사용 로직)
    // Toggle - 활성화/비활성화
    // 스킬에 따라 특정 기능을 사용 못하게 할 수도 있다
    // 드레인 끌어당기기 -> 금지
    // 위에서 Timing의 기능을 못쓰게 한다.
    [Serializable]
    public class ToggleSetting
    {
        [Header("ToggleType")]
        public ToggleType ToggleType;
        [Header("Toggle -> Collision")]
        public bool On;
        public EState Timing;  // 타이밍
        [Header("Toggle -> Function")]
        public string Name;
    }
    #endregion

    public void UpdateLevel(int level)
    {
        // 레벨에 따라 감소했던것도 증가해야 함
        // 1. 기존의 정보를 빼주고 새로 다시 넣기 ex) 10, 20 -> -10 +20
        int curLevel = this.level;
        this.level = level; // 레벨을 최신으로 변경하고

        if (passiveType.Equals(PassiveType.Modify)) // Modify의 경우
        {
            ResetValue(curLevel);       //  우선 증가한 값을 빼주고
            SetValue();                 // 다시 넣어준다
        }
        else if(passiveType.Equals(PassiveType.Condition)) // Condition의 경우
        {
            ReturnValue(curLevel);    // 정보를 빼주면 StatModel에서 변화를 감지해서 다시 재검사를 함
        }
        // 다시 한번 확인
        statModel.AllCheck();
        Debug.Log($"<color=blue>{parent.Name}스킬 갱신!</color>");
    }
}

[Serializable]
public class PassiveSkills
{
    public List<PassiveSkill> passiveSkills;
}
