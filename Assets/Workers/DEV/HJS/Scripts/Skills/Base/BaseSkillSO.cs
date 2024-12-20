using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static SkillEnum;

[CreateAssetMenu(menuName = "Scriptables/Base_Skill")]
public class BaseSkillSO : ScriptableObject
{
    [Header("Skill_Info")]
    public new string Name;               // 스킬 이름
    public string Description;            // 스킬 설명
    public Sprite Icon;                   // 스킬 아이콘
    private int skillLevel;
    [Space]
    [Header("ActTiming")]
    public ActTimingType Timing;  // 타이밍
    [Space(2)]
    [Header("Effect_Active")]
    public List<ActiveSkillSO> EffectSkills;
    [Space(2)]
    [Header("Effect_Passive")]
    public List<PassiveSkillSO> PassiveSkills;

    [Space(3)]
    [Header("Test")]
    public List<ActiveSkill> activeSkills;
    public List<PassiveSkill> passiveSkills;
    public int SkillLevel { get { return skillLevel; } set { skillLevel = value; onChangeLevel?.Invoke(skillLevel); } }

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

        #region 프로퍼티
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
        public void Use(GameObject obj)
        {
            if(!skillSpecDatabase.GetData(parent, out SkillSpecDatabase.Spec testSpec))
            {
                Debug.Log($"<color=red>{obj.name}가 호출! 레벨이 {level}Lv인 {parent.name}을 사용하려고 했는데</color>");
                Debug.LogError("스킬 딕셔너리에 데이터가 없다!");
                return;
            }
            
            if (Create)
            {
                foreach (GameObject gameObject in createSetting.CreateObject)
                {
                    Debug.Log(gameObject.name);
                    GameObject game = Instantiate(gameObject, obj.transform.position + Vector3.up, Quaternion.identity);
                    // Level에 대한 정의
                    // 해당 오브젝트에게 값을 전달
                    game.GetComponent<ISpec>().SetSpec(testSpec, level);

                    // 설치물이 장판이라면
                    FloorSpawner flooring = game.GetComponent<FloorSpawner>();
                    if (flooring is not null)
                    {
                        flooring.SetTarget = obj.transform;
                        Debug.Log($"floor 부착! {obj.name}");
                    }
                    Debug.Log($"충돌한 {obj.name}의 위치에서 {game.name}을 생성하겠다!");
                }
            }

            // 상호작용
            if(Interaction)
            {
                // 충돌이 켜져있다 -> 충돌한 적한테 할 행동
                // 상호작용 리스트
                // 가이드 샷 -> 던진 물건이 가장 가까운 적에게 향해 날아간다.
                // 프로스트 샷 -> 상대를 느려지게 한다
                // 
                // 충돌이 꺼져있다 -> 나에게 적용할 행동
                // 상대와 나 IEffect
                // 나 혼자 -> 컴포넌트 On/Off -> 다른 옵션
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
        private SkillSpecDatabase skillSpecDatabase;
        private int level;
        private BaseSkillSO parent;

        [SerializeField] PassiveType passiveType;

        [Space(2)]
        [Header("Settings")]
        [SerializeField, ShowEnum((int)PassiveType.Modify, "passiveType")] ModifySetting modifySetting;
        [SerializeField, ShowEnum((int)PassiveType.Condition, "passiveType")] ConditionSetting conditionSetting;

        public SkillSpecDatabase SkillSpecDatabase { set { skillSpecDatabase = value; Debug.Log("<color=yellow>패시브 스킬 스펙SO 설정</color>"); } }
        public BaseSkillSO Parent { set { parent = value; Debug.Log("<color=yellow>패시브 스킬부모 설정</color>"); } }

        // Modify - 수정
        // 값의 수정을 담당
        // 해당 적용을 할 때 바로 state에게 적용할 거 같다
        // 대폭 이렇게 있지만 -> 공격력을 더해준다
        [Serializable]
        public class ModifySetting
        {
            // Stat의 값 조절
            // 플레이어의 기존 범위
            // 이건 기획팀에서의 필요한  스텟에 따라 달라질거 같다
        }

        // Condition - 조건
        // 값의 변경에 따라 행동 담당
        // 해당 내용은 MVC 에서 -> Model의 이벤트에 연결해서 사용할 거 같다.
        // 함수의 내용은 따로 정의 해야할 거 같다
        [Serializable]
        public class ConditionSetting
        {

        }
    }

    private void OnDestroy()
    {
        onChangeLevel.RemoveAllListeners();
        Debug.Log($"스킬 {name}가 파괴되었습니다");
    }

}
