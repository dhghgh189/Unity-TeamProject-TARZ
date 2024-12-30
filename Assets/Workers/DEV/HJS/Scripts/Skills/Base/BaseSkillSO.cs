using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SkillEnum;

[CreateAssetMenu(menuName = "Scriptables/Base_Skill")]
public class BaseSkillSO : ScriptableObject
{
    [Header("Skill_Info")]
    public string Name;               // 스킬 이름
    public string Description;            // 스킬 설명
    public Sprite Icon;                   // 스킬 아이콘
    [Range(1, 3)] public int MaxLevel;    // 레벨업 가능한 최대 레벨
    private int skillLevel;
    [Space]
    [Header("ActTiming")]
    public ActTimingType Timing;  // 타이밍
    [Space(3)]
    [Header("Effect_Active")]
    [Space(2)]
    public List<ActiveSkill> activeSkills;
    [Space(2)]
    [Header("Effect_Passive")]
    public List<PassiveSkill> passiveSkills;
    public int SkillLevel { get { return skillLevel; } set { skillLevel = value; onChangeLevel?.Invoke(skillLevel); } }

    [HideInInspector]
    public UnityEvent<int> onChangeLevel;
    [HideInInspector]
    public StatModel model;

    [Serializable]
    public class ActiveSkill
    {
        private int level;
        private BaseSkillSO parent;

        [Header("Skill_Defualt_Options")]
        [SerializeField] Target who;
        [SerializeField] RefeatType refeat;
        [SerializeField] ActConditionType when;

        [Header("SkillOptions")]
        public bool Create;
        public bool Interaction;

        [Space(2)]
        [Header("Settings")]
        [SerializeField] CreateSetting createSetting;
        [Space(2)]
        [SerializeField] InteractionSetting interactionSetting;
        [Space(5)]
        [Header("Skill_Data_By_Level")]
        [SerializeField] Spec skillLevelSpec;

        #region 액티브 프로퍼티
        public Target Target => who;
        public RefeatType Refeat => refeat;
        public ActConditionType CollisionType => when;
        public BaseSkillSO Parent { set { parent = value; Debug.Log("<color=yellow>액티브 스킬부모 설정</color>"); } }
        public void SetModel(StatModel statModel) => skillLevelSpec.statModel = statModel;
        #endregion

        [Serializable]
        public class CreateSetting
        {
            public List<GameObject> CreateObject; // 생성 여부가 True일 때 -> 생성할 오브젝트 (ex. 폭발, 독구름)
        }
        [Serializable]
        public class InteractionSetting
        {
            public InteractionType type;    // 상호작용 여부가 Trued리 때 -> IEffect를 적용할 내용
        }
        /// <summary>
        /// 요청을 하는 함수 
        /// </summary>
        /// <param name="requester">요청자</param>
        /// <param name="target">타겟, 없으면 null로 받아진다.</param>
        public void Use(GameObject requester, GameObject target = null)
        {

            if (Create)
            {
                foreach (GameObject gameObject in createSetting.CreateObject)
                {
                    Debug.Log(gameObject.name);
                    GameObject game = Instantiate(gameObject, requester.transform.position, Quaternion.identity);
                    // Level에 대한 정의
                    // 해당 오브젝트에게 값을 전달
                    game.GetComponent<ISpec>()?.SetSpec(skillLevelSpec, level);

                    // 설치물이 장판이라면
                    FloorSpawner flooring = game.GetComponent<FloorSpawner>();
                    if (flooring is not null)
                    {
                        flooring.SetTarget = requester.transform;
                        Debug.Log($"floor 부착! {requester.name}");
                    }
                    Debug.Log($"충돌한 {requester.name}의 위치에서 {game.name}을 생성하겠다!");
                }
            }

            // 상호작용
            if (Interaction)
            {
                // 새로운 상호작용을 만들어서
                Interaction interaction = new Interaction(interactionSetting.type);
                // 스킬의 레벨에 맞게 값을 설정하고
                interaction.SetSpec(skillLevelSpec, level);
                // 상호작용 하기
                interaction.Activate(requester, target);
            }
        }

        public void UpdateLevel(int level)
        {
            this.level = level;
        }
    }

    [Serializable]
    public class PassiveSkill
    {
        private StatModel statModel;
        private SkillSpecDatabase skillSpecDatabase;
        private int level;
        private BaseSkillSO parent;
        private Dictionary<ConditionType, Func<bool>> conditions;

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
        public BaseSkillSO Parent { set { parent = value; Debug.Log("<color=yellow>패시브 스킬부모 설정</color>"); } }
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
            [Tooltip("ex) 100% = 100, 50% = 50")] public float Amount;
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
                        statModel.MaxHp = GetModifySetting.Amount;
                        statModel.CurrentHp = statModel.CurrentHp;
                    }
                    else if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                    {
                        statModel.SetAbility(AdditionAbility.MaxHpPer, GetModifySetting.Amount);
                    }
                    break;
                case PassiveModifyType.MaxStamina:
                    tempValue = statModel.MaxStamina;
                    if (GetModifySetting.inputType == PassiveModifyInputType.Value)
                    {
                        statModel.MaxStamina = GetModifySetting.Amount;
                        statModel.CurrentStamina = statModel.CurrentStamina;
                    }
                    else if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                    {
                        statModel.SetAbility(AdditionAbility.MaxStaminaPer, GetModifySetting.Amount);
                    }
                    break;
                case PassiveModifyType.MoveSpeed:
                    tempValue = statModel.MoveSpeed;
                    if (GetModifySetting.inputType == PassiveModifyInputType.Value)
                    {
                        statModel.MoveSpeed = GetModifySetting.Amount;
                        Debug.Log("TestOn");
                    }
                    else if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                    {
                        statModel.SetAbility(AdditionAbility.MoveSpeedPer, GetModifySetting.Amount);
                    }
                    break;
                case PassiveModifyType.AllPower:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                    {
                        statModel.SetAbility(AdditionAbility.AllPowerPer, GetModifySetting.Amount);
                    }
                    else
                    {
                        Debug.LogWarning("<Color=red>추가 능력치는 수치를 값으로 설정할 수 없습니다.</color>");
                    }
                    break;
                case PassiveModifyType.DefaultPower:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                    {
                        statModel.SetAbility(AdditionAbility.DefaultPowerPer, GetModifySetting.Amount);
                    }
                    else Debug.LogWarning("<Color=red>추가 능력치는 수치를 값으로 설정할 수 없습니다.</color>");
                    break;
                case PassiveModifyType.StaminaCostRate:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Percent)
                    {
                        statModel.StaminaCostRate = Mathf.Clamp((GetModifySetting.Amount * 0.01f), 0f, 1f); break;
                    }
                    else Debug.LogWarning("<Color=red>추가 능력치는 수치를 값으로 설정할 수 없습니다.</color>");
                    break;
            }
            GetModifySetting.TempValue = tempValue;
        }
        /// <summary>
        /// 변경한 값을 원복해주는 함수
        /// </summary>
        public void ResetValue()
        {
            switch (GetModifySetting.ModifyType)
            {
                case PassiveModifyType.MaxHp:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Value) statModel.MaxHp = GetModifySetting.TempValue;
                    else if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.MaxHpPer, -(GetModifySetting.Amount));
                    break;
                case PassiveModifyType.MaxStamina:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Value) statModel.MaxStamina = GetModifySetting.TempValue;
                    else if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.MaxStaminaPer, -(GetModifySetting.Amount));
                    break;
                case PassiveModifyType.MoveSpeed:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Value) statModel.MoveSpeed = GetModifySetting.TempValue;
                    else if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.MoveSpeedPer, -(GetModifySetting.Amount));
                    break;
                case PassiveModifyType.AllPower:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.AllPowerPer, -(GetModifySetting.Amount));
                    break;
                case PassiveModifyType.DefaultPower:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.SetAbility(AdditionAbility.DefaultPowerPer, -(GetModifySetting.Amount));
                    break;
                case PassiveModifyType.StaminaCostRate:
                    if (GetModifySetting.inputType == PassiveModifyInputType.Percent) statModel.StaminaCostRate += Mathf.Clamp((1 - GetModifySetting.Amount * 0.01f), 0f, 1f);
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
            public float Amount;
        }

        /// <summary>
        /// 이벤트에 부착되어서 조건을 확인할 함수
        /// </summary>
        /// <param name="data">조건의 대상</param>
        public void ConditionCheck(float data)
        {
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

            if (conditions.TryGetValue(conditionSetting.Condition, out Func<bool> conditionsFunc))
            {
                if (conditionsFunc())
                {
                    // cs 8506 why?
                    // _ = conditionSetting.resultModifyType switch
                    // {
                    //     PassiveResultModifyType.Hp => statModel.MaxHp += (statModel.MaxHp * (conditionSetting.IsIncrease ? 1 : -1) * conditionSetting.Amount),
                    //     PassiveResultModifyType.Stemina => statModel.MaxStamina += (statModel.MaxStamina * (conditionSetting.IsIncrease ? 1 : -1) * conditionSetting.Amount),
                    //     PassiveResultModifyType.Power => statModel.SetAbility(AdditionAbility.AllPowerPer, statModel.GetAbility(AdditionAbility.AllPowerPer) * (conditionSetting.IsIncrease ? 1 : -1) * conditionSetting.Amount),
                    //     _ => throw new NotImplementedException(),
                    // };
                    if (conditionSetting.isChanged) return;

                    conditionSetting.isChanged = true;

                    switch (conditionSetting.resultModifyType)
                    {
                        case PassiveResultModifyType.MaxHp: Debug.Log("HP+"); statModel.SetAbility(AdditionAbility.MaxHpPer, (conditionSetting.Amount)); break;
                        case PassiveResultModifyType.MaxStamina: Debug.Log("ST+"); statModel.SetAbility(AdditionAbility.MaxStaminaPer, (conditionSetting.Amount)); break;
                        case PassiveResultModifyType.AllPower: Debug.Log("AP+"); statModel.SetAbility(AdditionAbility.AllPowerPer, (conditionSetting.Amount)); break;
                        case PassiveResultModifyType.DefaultPower: Debug.Log("DP+"); statModel.SetAbility(AdditionAbility.DefaultPowerPer, (conditionSetting.Amount)); break;
                        case PassiveResultModifyType.StaminaCostRate: Debug.Log("SC+"); statModel.StaminaCostRate = Mathf.Clamp(conditionSetting.Amount * 0.01f, 0f, 1f); break;
                    }
                }
                else if (conditionSetting.isChanged)
                {
                    conditionSetting.isChanged = false;
                    switch (conditionSetting.resultModifyType)
                    {
                        case PassiveResultModifyType.MaxHp: Debug.Log("HP-"); statModel.SetAbility(AdditionAbility.MaxHpPer, (-conditionSetting.Amount)); break;
                        case PassiveResultModifyType.MaxStamina: Debug.Log("ST-"); statModel.SetAbility(AdditionAbility.MaxStaminaPer, (-conditionSetting.Amount)); break;
                        case PassiveResultModifyType.AllPower: Debug.Log("AP-"); statModel.SetAbility(AdditionAbility.AllPowerPer, (-conditionSetting.Amount)); break;
                        case PassiveResultModifyType.DefaultPower: Debug.Log("DP-"); statModel.SetAbility(AdditionAbility.DefaultPowerPer, (-conditionSetting.Amount)); break;
                        case PassiveResultModifyType.StaminaCostRate: Debug.Log("SC-"); statModel.StaminaCostRate += Mathf.Clamp(1 - conditionSetting.Amount * 0.01f, 0f, 1f); break;
                    }
                }
            }
            else
            {
                Debug.LogError($"{parent.Name}의 passive 스킬의 범위가 없습니다!");
            }
        }

        public void ReturnValue()
        {
            if (conditionSetting.isChanged)
            {
                switch (conditionSetting.resultModifyType)
                {
                    case PassiveResultModifyType.MaxHp: Debug.Log("HP-"); statModel.SetAbility(AdditionAbility.MaxHpPer, (-conditionSetting.Amount)); break;
                    case PassiveResultModifyType.MaxStamina: Debug.Log("ST-"); statModel.SetAbility(AdditionAbility.MaxStaminaPer, (-conditionSetting.Amount)); break;
                    case PassiveResultModifyType.AllPower: Debug.Log("AP-"); statModel.SetAbility(AdditionAbility.AllPowerPer, (-conditionSetting.Amount)); break;
                    case PassiveResultModifyType.DefaultPower: Debug.Log("DP-"); statModel.SetAbility(AdditionAbility.DefaultPowerPer, (-conditionSetting.Amount)); break;
                    case PassiveResultModifyType.StaminaCostRate: Debug.Log("SC-"); statModel.StaminaCostRate = Mathf.Lerp(0f, 1f, 1 - conditionSetting.Amount * 0.01f); break;
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
            [Header("Toggle -> Function")]
            public string Name;
        }
        #endregion

    }


    private void OnDestroy()
    {
        onChangeLevel.RemoveAllListeners();
        Debug.Log($"스킬 {name}가 파괴되었습니다");
    }

    [Serializable]
    public struct Spec
    {
        [HideInInspector]
        public StatModel statModel;
        [Header("Active")]
        [SerializeField] List<float> power;
        [SerializeField] List<float> range;
        [SerializeField] List<float> time;
        [Header("Interaction")]
        [SerializeField, Range(0f, 1f)] List<float> degree;
        [SerializeField] List<float> damage;
        [SerializeField] List<float> duration;

        public float Power(int level) => power[level - 1] * statModel.SkillPowerPer;
        public float Range(int level) => range[level - 1];
        public float Time(int level) => time[level - 1];

        public float interactioDegree(int level) { return (degree.Count > 0) ? degree[level - 1] * statModel.SkillPowerPer : 0; }
        public float InteractionDuration(int level) { return (duration.Count > 0) ? duration[level - 1] : 0; }
        public float InteractionDamage(int level) { return (damage.Count > 0) ? damage[level - 1] : 0; }

    }
}

