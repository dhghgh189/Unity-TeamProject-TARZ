using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SkillEnum;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "Scriptables/Base_Skill")]
public class BaseSkillSO : ScriptableObject
{
    [Header("Skill_Info")]
    public new string Name;               // 스킬 이름
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

    [Serializable]
    public class ActiveSkill
    {
        private SkillSpecDatabase skillSpecDatabase;
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

        #region 액티브 프로퍼티
        public Target Target => who;
        public RefeatType Refeat => refeat;
        public ActConditionType CollisionType => when;
        public SkillSpecDatabase SkillSpecDatabase { set { skillSpecDatabase = value; Debug.Log("<color=yellow>액티브 스킬 스펙SO 설정</color>"); } }
        public BaseSkillSO Parent { set { parent = value; Debug.Log("<color=yellow>액티브 스킬부모 설정</color>"); } }
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
            if (!skillSpecDatabase.GetData(parent, out SkillSpecDatabase.Spec testSpec))
            {
                Debug.Log($"<color=red>{requester.name}가 호출! 레벨이 {level}Lv인 {parent.name}을 사용하려고 했는데</color>");
                Debug.LogError("스킬 딕셔너리에 데이터가 없다!");
                return;
            }

            if (Create)
            {
                foreach (GameObject gameObject in createSetting.CreateObject)
                {
                    Debug.Log(gameObject.name);
                    GameObject game = Instantiate(gameObject, requester.transform.position + Vector3.up, Quaternion.identity);
                    // Level에 대한 정의
                    // 해당 오브젝트에게 값을 전달
                    game.GetComponent<ISpec>()?.SetSpec(testSpec, level);

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
                interaction.SetSpec(testSpec, level);
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
            // 플레이어의 기존 범위
            public PassiveModifyType ModifyType;
            public float value;
            public float tempValue;
        }

        /// <summary>
        /// 값을 변경해줄 함수
        /// </summary>
        public void SetValue()
        {
            float tempValue = 0f;
            switch(GetModifySetting.ModifyType)
            {
                case PassiveModifyType.Hp: tempValue = statModel.MaxHp; statModel.MaxHp = GetModifySetting.value; break;
                case PassiveModifyType.Stamina: tempValue = statModel.MaxStamina; statModel.MaxStamina = GetModifySetting.value; break;
            }
            GetModifySetting.tempValue = tempValue;
        }

        public void ResetValue()
        {
            switch (GetModifySetting.ModifyType)
            {
                case PassiveModifyType.Hp: statModel.MaxHp += GetModifySetting.tempValue; break;
                case PassiveModifyType.Stamina: statModel.MaxStamina += GetModifySetting.tempValue; break;
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
                    switch (conditionSetting.resultModifyType)
                    {
                        case PassiveResultModifyType.Hp: Debug.Log("HP"); statModel.SetAbility(AdditionAbility.MaxHpPer,(conditionSetting.Amount)); break;
                        case PassiveResultModifyType.Stamina: Debug.Log("ST"); statModel.SetAbility(AdditionAbility.MaxStaminaPer, (conditionSetting.Amount)); break;
                        case PassiveResultModifyType.DefaultPower: Debug.Log("DP"); statModel.SetAbility(AdditionAbility.DefaultPowerPer, (conditionSetting.Amount)); break;
                    }
                    
                }
            }
            else
            {
                Debug.LogError($"{parent.Name}의 passive 스킬의 범위가 없습니다!");
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
}

