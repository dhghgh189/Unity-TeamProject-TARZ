using BehaviorDesigner.Runtime.Tasks.Unity.UnityLight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ActTiming = SkillEnum.ActTimingType;
using static SkillEnum;
using static BaseSkillSO;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;

public class PlayerSkillHandler : MonoBehaviour
{
    [Header("Evnets")]
    private UnityEvent<GameObject>[] onActionPlayerEvents;         // 기본 상태에서의 할일 - Enter
    private UnityEvent<GameObject>[] onCollisionPlayerEvents;      // 상태에서의 충돌 - OnCollision or OnTrigger
    private UnityEvent<GameObject> onCollisionThrowObjectEvents;   // ThrowObject의 충돌 - OnCollision or OnTrigger
    private UnityEvent<GameObject> onActionThrowObjectEvents;      // 기본 ThrowObject에서의 할일 - Enter

    [Header("SkillList")]
    [SerializeField] Dictionary<string, (bool, int)> skillList;
    

    [Header("Test")]
    [SerializeField] bool isTrue;
    [SerializeField] SkillSpecDatabase skillSpec;
    [SerializeField] Dictionary<BaseSkillSO, int> skillDic;

    private void Start()
    {
        skillSpec = Instantiate(skillSpec);

        onActionPlayerEvents = new UnityEvent<GameObject>[(int)ActTiming.None];
        onCollisionPlayerEvents = new UnityEvent<GameObject>[(int)ActTiming.None];
        onCollisionThrowObjectEvents = new UnityEvent<GameObject>();
        onActionThrowObjectEvents = new UnityEvent<GameObject>();

        for (int i = 0; i < (int)ActTiming.None; i++)
        {
            onActionPlayerEvents[i] = new UnityEvent<GameObject>();
            onCollisionPlayerEvents[i] = new UnityEvent<GameObject>();
        }
        skillList = new Dictionary<string, (bool, int)>();
        skillDic = new Dictionary<BaseSkillSO, int>();
    }

    // 플레이어 -> 헨들러에게 스킬 사용 요청
    public void Use(ActTiming act) => onActionPlayerEvents[(int)act]?.Invoke(gameObject);
    // 던지는 물체 -> 헨들러에게 스킬 사용 요청
    public void Use(GameObject throwObject) => onActionThrowObjectEvents?.Invoke(throwObject);
    // 플레이어 -> 헨들러에게 충돌 되었다고 요청
    public void PlayerCollision(ActTiming act, GameObject collider) => onCollisionPlayerEvents[(int)act]?.Invoke(collider);
    // 던지는 물체 -> 헨들러에게 충돌 되었다고 요청
    public void ThrowObjectCollision(GameObject collider) => onCollisionThrowObjectEvents?.Invoke(collider);

    public void AddSkill(BaseSkillSO skill)
    {
        if (!isTrue)
        {
            // 스킬안에 있는 능력들을 전부 붙여주기
            foreach (ActiveSkillSO set in skill.EffectSkills)
            {
                // 스킬의 사용 주체에 따라 실행
                switch (set.Target)
                {
                    // 플레이어
                    case Target.Player:
                        // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                        if (set.CollisionType == ActConditionType.Collision)
                        {
                            onCollisionPlayerEvents[(int)skill.Timing].AddListener(set.Use);
                        }
                        // 아니라면 -> 기본 이벤트로 연결
                        else
                        {
                            onActionPlayerEvents[(int)skill.Timing].AddListener(set.Use);
                        }
                        break;
                    // 던지는 물체
                    case Target.ThrowObject:
                        // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                        if (set.CollisionType == ActConditionType.Collision)
                        {
                            onCollisionThrowObjectEvents.AddListener(set.Use);
                        }
                        // 아니라면 -> 기본 이벤트로 연결
                        else
                        {
                            // TODO: 호출 할 곳 구현
                            onActionThrowObjectEvents.AddListener(set.Use);
                        }
                        break;
                }
            }
        }
        else
        {
            foreach (ActiveSkill set in skill.activeSkills)
            {
                // 해당 스킬의 레벨에 따라 변경하기 위해 부모 설정
                set.Parent = skill;
                // 레벨 변경의 대한 수치를 가져오기 위한 데이터베이스 넣기
                set.SkillSpecDatabase = skillSpec;

                skill.onChangeLevel.AddListener(set.UpdateLevel);

                // 스킬의 사용 주체에 따라 실행
                switch (set.Target)
                {
                    // 플레이어
                    case Target.Player:
                        // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                        if (set.CollisionType == ActConditionType.Collision)
                        {
                            onCollisionPlayerEvents[(int)skill.Timing].AddListener(set.Use);
                        }
                        // 아니라면 -> 기본 이벤트로 연결
                        else
                        {
                            onActionPlayerEvents[(int)skill.Timing].AddListener(set.Use);
                        }
                        break;
                    // 던지는 물체
                    case Target.ThrowObject:
                        // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                        if (set.CollisionType == ActConditionType.Collision)
                        {
                            onCollisionThrowObjectEvents.AddListener(set.Use);
                        }
                        // 아니라면 -> 기본 이벤트로 연결
                        else
                        {
                            // TODO: 호출 할 곳 구현
                            onActionThrowObjectEvents.AddListener(set.Use);
                        }
                        break;
                }
            }
        }

        // 스킬리스트에 있으면 레벨 올려주기
        if (skillList.ContainsKey(skill.Name))
        {
            (bool, int) value = skillList[skill.Name];
            value.Item1 = true; value.Item2++;
            skillList[skill.Name] = value;

            int level = skillDic[skill];
            level += 1;
            skillDic[skill] = level;
            skill.SkillLevel = level;
        }
        // 스킬리스트에 없으면 넣어두기
        else
        {
            skillList.Add(skill.Name, (true, 1));

            skillDic.Add(skill, 1);
            skill.SkillLevel = 1;
        }
        // 디버그로 정보 보여주기
        Debug.Log($"Add Skill Name : {skill.Name}  / Skill Act Timing : {skill.Timing} ");
    }

    public void RemoveSkill(BaseSkillSO skill)
    {
        // 스킬이 없을 때 예외처리
        if (skill is null) return;

        // 스킬안에 능력들 전부 회수하기
        foreach (ActiveSkillSO set in skill.EffectSkills)
        {
            switch (set.Target)
            {
                // 플레이어
                case Target.Player:
                    // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                    if (set.CollisionType == ActConditionType.Collision)
                    {
                        onCollisionPlayerEvents[(int)skill.Timing].RemoveListener(set.Use);
                    }
                    // 아니라면 -> 기본 이벤트로 연결
                    else
                    {
                        onActionPlayerEvents[(int)skill.Timing].RemoveListener(set.Use);
                    }
                    break;
                // 던지는 물체
                case Target.ThrowObject:
                    // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                    if (set.CollisionType == ActConditionType.Collision)
                    {
                        onCollisionThrowObjectEvents.RemoveListener(set.Use);
                    }
                    // 아니라면 -> 기본 이벤트로 연결
                    else
                    {
                        // TODO: 호출 할 곳 구현
                        onActionThrowObjectEvents.RemoveListener(set.Use);
                    }
                    break;
            }
        }

        // 스킬 리스트에서 제거하기
        // TODO: 스킬 레벨에 따라 능력 변화하기
        skillList[skill.Name] = (false, 0);
        Debug.Log($"Remove Active Skill Name : {skill.Name}  / Skill Act Timing : {skill.Timing}");
        Destroy(skill);
    }

    // 플레이어가 사망 -> 파괴되었을 때
    private void OnDestroy()
    {
        // 이벤트들에 달려있는 모든 리스터 연결 종료
        onCollisionThrowObjectEvents.RemoveAllListeners();
        onActionThrowObjectEvents.RemoveAllListeners();

        for (int i = 0; i < (int)ActTiming.None; i++)
        {
            onActionPlayerEvents[i].RemoveAllListeners();
            onCollisionPlayerEvents[i].RemoveAllListeners();
        }
    }
}
