using System;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

[Serializable]
public class ActiveSkill
{
    [SerializeField] string IndexName;
    [Tooltip("Player = 플레이어 / ThrowObject = 던지는 물체")]
    public Target Target;
    private int level;
    private BaseSkillSO parent;

    [Header("Target => Player")]
    [Tooltip("스킬이 발동할 수 있는 행동 조건")] public EState ConditionState;              // 플레이어의 행동 조건
    public ActionTimingType ActTiming;                   // 발동 시점
    [Space(2)]
    [Header("Target => ThrowObject")]
    public ActConditionType ConditionType;
    [Space(3)]
    [Header("생성")]
    public bool Create;                       // 생성 여부
    [SerializeField] GameObject createObject; // 생성 여부가 True일 때 -> 생성할 오브젝트 (ex. 폭발, 독구름)
    [Tooltip("X: 데미지, Y: 범위, Z: 시간")]
    public Vector3[] performance_Act;         // 스킬의 정보
    [Space(5)]
    [Header("상태 이상 효과")]
    public bool Interaction;                  // 상태이상 효과 여부
    public InteractionType type;              // 상태이상 종류
    [Tooltip("X: 데미지, Y: 비율, Z: 시간")]
    public Vector3[] performance_Interaction; // 스킬의 정보
    [Space(5)]
    [Header("특수 효과")]
    public bool UniqueEffect;                 // 특수 효과 여부
    public UniqueEffectType status;                // 특수 효과를 적용할 함수의 종류
    public GameObject UniqueEffectObject;     // 특수 효과가 들어있는 함수

    private Spec skillLevelSpec;              // 입력한 스펙이 저장되는 구조체
    public BaseSkillSO Parent { set { parent = value; Debug.Log("<color=yellow>액티브 스킬부모 설정</color>"); level = parent.SkillLevel; } }
    public void SetModel(StatModel statModel) => skillLevelSpec.statModel = statModel;

    public void Use(GameObject requester, GameObject target = null)
    {
        skillLevelSpec.ActValues = performance_Act;
        skillLevelSpec.InteractionValues = performance_Interaction;

        if (Create)
        {
            Debug.Log(createObject.name);
            GameObject game = UnityEngine.Object.Instantiate(createObject, requester.transform.position + Vector3.up, Quaternion.identity);
            game.GetComponent<ISpec>()?.SetSpec(skillLevelSpec, level);

            FloorSpawner flooring = game.GetComponent<FloorSpawner>();
            // 설치물이 장판이라면 -> 상태이상 스테이트 사용해야 한다.
            if (flooring is not null)
            {
                Spec skillLevelSpec = new();
                skillLevelSpec.statModel = this.skillLevelSpec.statModel;
                skillLevelSpec.InteractionValues = performance_Act;
                flooring.SetSpec(skillLevelSpec, level);
                flooring.SetTarget = requester.transform;
            }
        }

        // 상호작용
        if (Interaction)
        {
            // 새로운 상호작용을 만들어서
            Interaction interaction = new Interaction(type);
            // 스킬의 레벨에 맞게 값을 설정하고
            interaction.SetSpec(skillLevelSpec, level);
            // 상호작용 하기
            interaction.Activate(requester, target);
        }

        // 특수 효과
        if (UniqueEffect && !status.Equals(UniqueEffectType.None))
        {
            // TODO: 특수 효과
            // UniqueObject의 스크립트에서 Test_Status에 맞는 함수 실행
            // 매개변수로 Test_Status를 넘겨줘서 실행
        }
    }
    public void UpdateLevel(int level)
    {
        this.level = level;
    }
}

[Serializable]
public class ActiveSkills
{
    public List<ActiveSkill> activeSkills;
}

