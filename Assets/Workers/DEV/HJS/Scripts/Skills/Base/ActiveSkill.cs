using System;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

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
                GameObject game = GameObject.Instantiate(gameObject, requester.transform.position, Quaternion.identity);
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
