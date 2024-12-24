using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using static BaseSkillSO;
using static SkillEnum;
using ActTiming = SkillEnum.ActTimingType;

public class PlayerSkillHandler : MonoBehaviour
{
    [Header("Evnets")]
    private UnityEvent<GameObject, GameObject>[] onActionPlayerEvents;         // 기본 상태에서의 할일 - Enter
    private UnityEvent<GameObject, GameObject>[] onCollisionPlayerEvents;      // 상태에서의 충돌 - OnCollision or OnTrigger
    private UnityEvent<GameObject, GameObject> onCollisionThrowObjectEvents;   // ThrowObject의 충돌 - OnCollision or OnTrigger
    private UnityEvent<GameObject, GameObject> onActionThrowObjectEvents;      // 기본 ThrowObject에서의 할일 - Enter

    [SerializeField] DrainManager drainManager;
    [SerializeField] PlayerMovement playerMovement;

    [Header("SkillList")]
    [SerializeField] Dictionary<BaseSkillSO, int> skillDic;

    [Header("Test")]
    [Inject]
    [SerializeField] StatModel model;
    [Inject]
    [SerializeField] AblityAdapter adapter;

    private void Start()
    {
        onActionPlayerEvents = new UnityEvent<GameObject, GameObject>[(int)ActTiming.None];
        onCollisionPlayerEvents = new UnityEvent<GameObject, GameObject>[(int)ActTiming.None];
        onCollisionThrowObjectEvents = new UnityEvent<GameObject, GameObject>();
        onActionThrowObjectEvents = new UnityEvent<GameObject, GameObject>();

        for (int i = 0; i < (int)ActTiming.None; i++)
        {
            onActionPlayerEvents[i] = new UnityEvent<GameObject, GameObject>();
            onCollisionPlayerEvents[i] = new UnityEvent<GameObject, GameObject>();
        }
        skillDic = new Dictionary<BaseSkillSO, int>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            model.CurrentHp -= 10;
        }
    }

    // 플레이어 -> 헨들러에게 스킬 사용 요청
    public void Use(ActTiming act) => onActionPlayerEvents[(int)act]?.Invoke(gameObject, null);
    // 던지는 물체 -> 헨들러에게 스킬 사용 요청
    public void Use(GameObject throwObject) => onActionThrowObjectEvents?.Invoke(throwObject, null);
    // 플레이어 -> 헨들러에게 충돌 되었다고 요청
    public void PlayerCollision(ActTiming act, GameObject collider) => onCollisionPlayerEvents[(int)act]?.Invoke(gameObject, collider);
    // 던지는 물체 -> 헨들러에게 충돌 되었다고 요청
    public void ThrowObjectCollision(GameObject to, GameObject collider) => onCollisionThrowObjectEvents?.Invoke(to, collider);

    public void AddSkill(BaseSkillSO skill)
    {
        #region 액티브 스킬 넣기
        foreach (ActiveSkill actSkill in skill.activeSkills)
        {
            // 해당 스킬의 레벨에 따라 변경하기 위해 부모 설정
            actSkill.Parent = skill;
            // 레벨 변경의 대한 수치를 가져오기 위한 데이터베이스 넣기
            actSkill.SetModel(model);

            skill.onChangeLevel.AddListener(actSkill.UpdateLevel);

            // 스킬의 사용 주체에 따라 실행
            switch (actSkill.Target)
            {
                // 플레이어
                case Target.Player:
                    // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                    if (actSkill.CollisionType == ActConditionType.Collision)
                    {
                        onCollisionPlayerEvents[(int)skill.Timing].AddListener(actSkill.Use);
                    }
                    // 아니라면 -> 기본 이벤트로 연결
                    else
                    {
                        onActionPlayerEvents[(int)skill.Timing].AddListener(actSkill.Use);
                    }
                    break;
                // 던지는 물체
                case Target.ThrowObject:
                    // 충돌 설정이 되어 있다면 -> 충돌 이벤트로 연결
                    if (actSkill.CollisionType == ActConditionType.Collision)
                    {
                        onCollisionThrowObjectEvents.AddListener(actSkill.Use);
                    }
                    // 아니라면 -> 기본 이벤트로 연결
                    else
                    {
                        // TODO: 호출 할 곳 구현
                        onActionThrowObjectEvents.AddListener(actSkill.Use);
                    }
                    break;
            }
        }
        #endregion

        #region 패시브 스킬 넣기
        foreach(PassiveSkill psivSkill in skill.passiveSkills)
        {
            psivSkill.Parent = skill;
            psivSkill.StatModel = model;

            // 패시브 스킬의 종류에 따라 실행
            switch (psivSkill.GetPassiveType)
            {
                // 수정 - 직접적으로 값을 수정한다.
                case PassiveType.Modify:
                    switch (psivSkill.GetModifySetting.ModifyType)
                    {
                        case PassiveModifyType.DashTime:
                            playerMovement.DashTime += psivSkill.GetModifySetting.Amount;
                            break;
                        case PassiveModifyType.DrainRadius:
                            drainManager.MaxRadius += psivSkill.GetModifySetting.Amount;
                            break;
                        default:
                            psivSkill.SetValue();
                            break;
                    }
                    model.AllCheck();
                    break;
                // 조건 - 조건에 맞으면 특정 행동을 수행한다.
                case PassiveType.Condition:
                    switch (psivSkill.GetConditionSetting.modifyType)
                    {
                        case PassiveModifyType.MaxHp:
                            psivSkill.GetConditionSetting.MaxValue = model.MaxHp;
                            model.OnMaxHpChange += psivSkill.GetConditionSetting.SetMax;    // 최대 체력 연결
                            model.OnCurHpChange += psivSkill.ConditionCheck;                // 현재 체력 연결
                            Debug.Log("hp 스킬 연결!");
                            break;
                        case PassiveModifyType.MaxStamina:
                            psivSkill.GetConditionSetting.MaxValue = model.MaxStamina;
                            model.OnMaxStaminaChange += psivSkill.GetConditionSetting.SetMax;
                            model.OnCurStaminaChange += psivSkill.ConditionCheck;
                            Debug.Log("스테미너 스킬 연결!");
                            break;
                    }
                    model.AllCheck();
                    break;
                // 활성화/비활성화 - 특정 기능의 활성화 여부 설정한다.
                case PassiveType.Toggle:
                    switch (psivSkill.GetToggleSetting.ToggleType)
                    {
                        case ToggleType.Collision:
                            adapter.SetOnPlayerCollision((EState)Enum.Parse(typeof(EState), skill.Timing.ToString()), psivSkill.GetToggleSetting.On);
                            break;
                        case ToggleType.Function:
                            adapter.SetEnable(psivSkill.GetToggleSetting.Name);
                            break;
                    }
                    break;
            }
        }
        #endregion

        // 스킬리스트에 있으면 레벨 올려주기
        if (skillDic.ContainsKey(skill) )
        {
            int level = skillDic[skill];
            level += 1;
            skillDic[skill] = level;
            skill.SkillLevel = level;
        }
        // 스킬리스트에 없으면 넣어두기
        else
        {
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

        foreach (ActiveSkill actSkill in skill.activeSkills)
        {
            // 해당 스킬의 레벨에 따라 변경하기 위해 부모 설정
            actSkill.Parent = skill;
            // 레벨 변경의 대한 수치를 가져오기 위한 데이터베이스 넣기
            actSkill.SetModel(model);

            skill.onChangeLevel.RemoveListener(actSkill.UpdateLevel);

            // 스킬의 사용 주체에 따라 실행
            switch (actSkill.Target)
            {
                case Target.Player:
                    if (actSkill.CollisionType == ActConditionType.Collision)
                    {
                        onCollisionPlayerEvents[(int)skill.Timing].RemoveListener(actSkill.Use);
                    }
                    else
                    {
                        onActionPlayerEvents[(int)skill.Timing].RemoveListener(actSkill.Use);
                    }
                    break;
                case Target.ThrowObject:
                    if (actSkill.CollisionType == ActConditionType.Collision)
                    {
                        onCollisionThrowObjectEvents.RemoveListener(actSkill.Use);
                    }
                    else
                    {
                        onActionThrowObjectEvents.RemoveListener(actSkill.Use);
                    }
                    break;
            }
        }

        #region 패시브 스킬 빼기
        foreach (PassiveSkill psivSkill in skill.passiveSkills)
        {
            psivSkill.Parent = skill;

            // 패시브 스킬의 종류에 따라 실행
            switch (psivSkill.GetPassiveType)
            {
                // 수정 - 직접적으로 값을 수정한다.
                case PassiveType.Modify:
                    switch (psivSkill.GetModifySetting.ModifyType)
                    {
                        case PassiveModifyType.DashTime:
                            playerMovement.DashTime -= psivSkill.GetModifySetting.Amount;
                            break;
                        case PassiveModifyType.DrainRadius:
                            drainManager.MaxRadius -= psivSkill.GetModifySetting.Amount;
                            break;
                        default:
                            psivSkill.ResetValue();
                            break;
                    }
                    model.AllCheck();
                    break;
                // 조건 - 조건에 맞으면 특정 행동을 수행한다.
                case PassiveType.Condition:
                    psivSkill.ReturnValue();
                    switch (psivSkill.GetConditionSetting.modifyType)
                    {
                        case PassiveModifyType.MaxHp:
                            model.OnMaxHpChange -= psivSkill.GetConditionSetting.SetMax;    // 최대 값 연결 해제
                            model.OnCurHpChange -= psivSkill.ConditionCheck;                // 현재 값 연결 해제
                            break;
                        case PassiveModifyType.MaxStamina:
                            model.OnMaxStaminaChange -= psivSkill.GetConditionSetting.SetMax;
                            model.OnCurStaminaChange -= psivSkill.ConditionCheck;
                            break;
                    }
                    model.AllCheck();
                    break;
                // 활성화/비활성화 - 특정 기능의 활성화 여부 설정한다.
                case PassiveType.Toggle:
                    switch (psivSkill.GetToggleSetting.ToggleType)
                    {
                        case ToggleType.Collision:
                            adapter.SetOffPlayerCollision((EState)Enum.Parse(typeof(EState), skill.Timing.ToString()), psivSkill.GetToggleSetting.On);
                            break;
                        case ToggleType.Function:
                            adapter.SetDisable(psivSkill.GetToggleSetting.Name);
                            break;
                    }
                    break;
            }
        }
        #endregion

        // 스킬 리스트에서 제거하기
        skillDic[skill] = 0;
        Debug.Log($"Remove Active Skill Name : {skill.Name}  / Skill Act Timing : {skill.Timing}");
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

    public void LevelUp(BaseSkillSO skill)
    {
        if (skillDic.ContainsKey(skill))
        {
            int level = skillDic[skill];

            if (level >= skill.MaxLevel)
            {
                return;
            }

            level += 1;
            skillDic[skill] = level;
            skill.SkillLevel = level;

            Debug.Log($"<color=white>{skill.name} 스킬 {level}로 레벨업!</color>");
        }
    }
}
