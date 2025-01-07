using System;
using System.Collections.Generic;
using UnityEngine;
using static SkillEnum;

public enum Test_Timing { Enter, Update, Exit, Act, Collision }
public enum Test_Status { None, Success, Failure }
[Serializable]
public class ActiveSkillSO
{
    [SerializeField] string IndexName;
    private int level;
    private BaseSkillSO parent;

    public Test_Timing act;                   // 발동 시점
    [Header("생성")]
    public bool Create;                       // 생성 여부
    [SerializeField] GameObject createObject; // 생성 여부가 True일 때 -> 생성할 오브젝트 (ex. 폭발, 독구름)
    public Vector3[] performance_Act;         // 스킬의 정보
    [Space(5)]
    [Header("상태 이상 효과")]
    public bool Interaction;                  // 상태이상 효과 여부
    public InteractionType type;              // 상태이상 종류
    public Vector3[] performance_Interaction; // 스킬의 정보
    [Space(5)]
    [Header("특수 효과")]
    public bool UniqueEffect;                 // 특수 효과 여부
    public Test_Status status;                // 특수 효과를 적용할 함수의 종류
    public GameObject UniqueEffectObject;     // 특수 효과가 들어있는 함수

    public void Use(GameObject requester, GameObject target = null)
    {
        if (Create)
        {
            Debug.Log(createObject.name);
            GameObject game = UnityEngine.Object.Instantiate(createObject, requester.transform.position + Vector3.up, Quaternion.identity);
            FloorSpawner flooring = game.GetComponent<FloorSpawner>();
            // 설치물이 장판이라면
            if (flooring is not null)
            {
                flooring.SetTarget = requester.transform;
                Debug.Log($"floor 부착! {requester.name}");
            }
            Debug.Log($"충돌한 {requester.name}의 위치에서 {game.name}을 생성하겠다!");
        }

        // 상호작용
        if (Interaction)
        {
            Spec skillLevelSpec = new Spec();
            // 새로운 상호작용을 만들어서
            Interaction interaction = new Interaction(type);
            // 스킬의 레벨에 맞게 값을 설정하고
            interaction.SetSpec(skillLevelSpec, level);
            // 상호작용 하기
            interaction.Activate(requester, target);
        }

        // 특수 효과
        if (UniqueEffect && !status.Equals(Test_Status.None))
        {

        }
    }
}

[Serializable]
public class ActiveSkills
{
    public List<ActiveSkillSO> activeSkillSOs;
}
