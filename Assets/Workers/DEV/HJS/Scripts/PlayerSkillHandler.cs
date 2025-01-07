using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using static SkillEnum;
using ActTiming = SkillEnum.ActTimingType;

public class PlayerSkillHandler : MonoBehaviour
{
    [Header("Evnets")]
    private UnityEvent<GameObject, GameObject> onCollisionThrowObjectEvents;   // ThrowObject의 충돌 - OnCollision or OnTrigger
    private UnityEvent<GameObject, GameObject> onActionThrowObjectEvents;      // 기본 ThrowObject에서의 할일 - Enter
    private Dictionary<ActionTimingType, UnityEvent<GameObject, GameObject>[]> eventDic;

    public SkillContainer Container;

    private DrainManager drainManager;
    private PlayerMovement playerMovement;

    [Header("SkillList")]
    [SerializeField] Dictionary<string, int> skillDic;

    [Header("Etc")]
    [Inject][SerializeField] StatModel model;
    [Inject] private AblityAdapter adapter;
    [Inject] private InGameSaveData saveData;

    private void Awake()
    {
        drainManager = GetComponentInChildren<DrainManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        eventDic = new Dictionary<ActionTimingType, UnityEvent<GameObject, GameObject>[]>
        {
            { ActionTimingType.Enter, new UnityEvent<GameObject, GameObject>[(int)EState.Length] },     // 행동에 입장했을 때
            { ActionTimingType.Update, new UnityEvent<GameObject, GameObject>[(int)EState.Length] },    // 행동 도중(플레이어 중심)
            { ActionTimingType.Act, new UnityEvent<GameObject, GameObject>[(int)EState.Length] },       // 행동 도중(기술 중심)
            { ActionTimingType.Exit, new UnityEvent<GameObject, GameObject>[(int)EState.Length] },      // 행동이 끝났을 때
            { ActionTimingType.Collision, new UnityEvent<GameObject, GameObject>[(int)EState.Length] }, // 행동 중 충돌했을 때
        };
        
        onCollisionThrowObjectEvents = new UnityEvent<GameObject, GameObject>();
        onActionThrowObjectEvents = new UnityEvent<GameObject, GameObject>();

        foreach(var value in eventDic.Values)
        {
            for (int i = 0; i < (int)EState.Length; i++)
            {
                value[i] = new UnityEvent<GameObject, GameObject>();
            }
        }

        skillDic = new Dictionary<string, int>();

        LoadSkills();
    }

    public void LoadSkills()
    {
        foreach (BlueChipSaveData bluechipData in saveData.blueChipSaveDatas)
        {
            AddSkill(bluechipData.BlueChipName, bluechipData.BlueChipLevel);
        }
    }

    // 던지는 물체 -> 헨들러에게 스킬 사용 요청
    public void Use(GameObject throwObject) => onActionThrowObjectEvents?.Invoke(throwObject, null);
    // 던지는 물체 -> 헨들러에게 충돌 되었다고 요청
    public void ThrowObjectCollision(GameObject to, GameObject collider) => onCollisionThrowObjectEvents?.Invoke(to, collider);
    // 플레이어 -> 스킬 사용 요쳥
    public void ActivateSkill(EState state, ActionTimingType act, GameObject collider = null)
    {
        if (eventDic == null) return;

        if(eventDic.TryGetValue(act, out var onResultEvent))
        {
            onResultEvent[(int)state]?.Invoke(gameObject, collider);
        }
    }

    public void CollisionEvent(EState curState, GameObject target) 
    {
        if (eventDic.TryGetValue(ActionTimingType.Collision, out UnityEvent<GameObject, GameObject>[] events))
        {
            events[(int)curState]?.Invoke(gameObject, target);
            Debug.Log($"{curState} 충돌 작동!");
        }
        else
        {
            Debug.Log($"{curState} 오작동!");
            return;
        }
    }

    public void AddSkill(string skillName, int setLevel = 1)
    {
        BaseSkillSO skill = Instantiate(Container.Skills.Where(x => x.Name == skillName).First());

        if (LevelUp(skill))
        {
            return;
        }

        // 스킬리스트에 있으면 레벨 올려주기 <- 제거했다가 다시 추가했을 때
        if (skillDic.ContainsKey(skill.Name))
        {
            Debug.Log("스킬 레벨업");
            int level = skillDic[skill.Name];
            level += setLevel;
            skillDic[skill.Name] = level;
            skill.SkillLevel = level;
            Debug.Log($"스킬 : {skill.Name} / {level}");
        }
        // 스킬리스트에 없으면 넣어두기
        else
        {
            skillDic.Add(skill.Name, setLevel);
            if (setLevel == 1)
            {
                skill.SkillLevel = 1;
            }
            Debug.Log("딕션에 추가!");
        }

        #region 액티브 스킬 넣기
        foreach (ActiveSkill actSkill in skill.ActiveSkills)
        {
            // 해당 스킬의 레벨에 따라 변경하기 위해 부모 설정
            actSkill.Parent = skill;
            // 레벨 변경의 대한 수치를 가져오기 위한 데이터베이스 넣기
            actSkill.SetModel(model);

            skill.onChangeLevel.AddListener(actSkill.UpdateLevel);

            if (actSkill.Target == Target.Player)
            {
                if (eventDic.TryGetValue(actSkill.ActTiming, out UnityEvent<GameObject, GameObject>[] onResultEvent))
                {
                    onResultEvent[(int)actSkill.ConditionState].AddListener(actSkill.Use);
                }
            }
            else if (actSkill.Target == Target.ThrowObject)
            {
                if (actSkill.ConditionType == ActConditionType.Start)
                    onActionThrowObjectEvents.AddListener(actSkill.Use);
                else if (actSkill.ConditionType == ActConditionType.Collision)
                    onCollisionThrowObjectEvents.AddListener(actSkill.Use);
            }
        }
        #endregion

        #region 패시브 스킬 넣기
        foreach (PassiveSkill psivSkill in skill.PassiveSkills)
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
                        case PassiveModifyType.DashSpeed:
                            model.DashSpeed += psivSkill.GetModifySetting.Amount(skill.SkillLevel);
                            break;
                        case PassiveModifyType.DrainRadius:
                            drainManager.MaxRadius += psivSkill.GetModifySetting.Amount(skill.SkillLevel);
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
                            adapter.SetOnPlayerCollision(psivSkill.GetToggleSetting.Timing, psivSkill.GetToggleSetting.On);
                            break;
                        case ToggleType.Function:
                            adapter.SetEnable(psivSkill.GetToggleSetting.Name);
                            break;
                    }
                    break;
            }
        }
        #endregion
        // 디버그로 정보 보여주기
        Debug.Log($"Add Skill Name : {skill.Name}  / Skill Tier : {skill.SkillTier} / Skill Level : {skill.SkillLevel}");
    }

    public List<BlueChipSaveData> SaveBlueChips()
    {
        List<BlueChipSaveData> blueChips = new();
        foreach (var item in skillDic)
        {
            blueChips.Add(new BlueChipSaveData() { BlueChipName = item.Key, BlueChipLevel = item.Value });
        }
        return blueChips;
    }

    public void RemoveSkill(BaseSkillSO skill)
    {
        // 스킬이 없을 때 예외처리
        if (skill is null) return;

        foreach (ActiveSkill actSkill in skill.ActiveSkills)
        {
            // 해당 스킬의 레벨에 따라 변경하기 위해 부모 설정
            actSkill.Parent = skill;
            // 레벨 변경의 대한 수치를 가져오기 위한 데이터베이스 넣기
            actSkill.SetModel(model);

            skill.onChangeLevel.RemoveListener(actSkill.UpdateLevel);

            // 스킬의 사용 주체에 따라 실행
            if (actSkill.Target == Target.Player)
            {
                if (eventDic.TryGetValue(actSkill.ActTiming, out UnityEvent<GameObject, GameObject>[] te))
                {
                    te[(int)actSkill.ConditionState].RemoveListener(actSkill.Use);
                }
            }
            else if (actSkill.Target == Target.ThrowObject)
            {
                if (actSkill.ConditionType == ActConditionType.Start)
                    onActionThrowObjectEvents.RemoveListener(actSkill.Use);
                else if (actSkill.ConditionType == ActConditionType.Collision)
                    onCollisionThrowObjectEvents.RemoveListener(actSkill.Use);
            }
        }

        #region 패시브 스킬 빼기
        foreach (PassiveSkill psivSkill in skill.PassiveSkills)
        {
            psivSkill.Parent = skill;

            // 패시브 스킬의 종류에 따라 실행
            switch (psivSkill.GetPassiveType)
            {
                // 수정 - 직접적으로 값을 수정한다.
                case PassiveType.Modify:
                    switch (psivSkill.GetModifySetting.ModifyType)
                    {
                        case PassiveModifyType.DashSpeed:
                            model.DashSpeed -= psivSkill.GetModifySetting.Amount(skill.SkillLevel);
                            break;
                        case PassiveModifyType.DrainRadius:
                            drainManager.MaxRadius -= psivSkill.GetModifySetting.Amount(skill.SkillLevel);
                            break;
                        default:
                            psivSkill.ResetValue();
                            break;
                    }
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
                    break;
                // 활성화/비활성화 - 특정 기능의 활성화 여부 설정한다.
                case PassiveType.Toggle:
                    switch (psivSkill.GetToggleSetting.ToggleType)
                    {
                        case ToggleType.Collision:
                            adapter.SetOffPlayerCollision(psivSkill.GetToggleSetting.Timing, psivSkill.GetToggleSetting.On);
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
        skillDic[skill.Name] = 0;
        // 스킬 제거하기
        Destroy(skill);
        Debug.Log($"Remove Active Skill Name : {skill.Name}");
    }

    // 플레이어가 사망 -> 파괴되었을 때
    private void OnDestroy()
    {
        Clear();
    }

    public void Clear()
    {
        // 이벤트들에 달려있는 모든 리스터 연결 종료
        onCollisionThrowObjectEvents.RemoveAllListeners();
        onActionThrowObjectEvents.RemoveAllListeners();

        if (eventDic == null) return;

        foreach (var item in eventDic.Values)
        {
            for (int i = 0; i < (int)EState.Length; i++)
            {
                item[i].RemoveAllListeners();
            }
        }

        skillDic.Clear();
    }

    public bool LevelUp(BaseSkillSO skill)
    {
        // 스킬에 등록이 되어있다면 -> 기존에 한번이라도 장착은 한 스킬
        if (skillDic.ContainsKey(skill.Name))
        {
            // 해당 스킬의 레벨을 가져온다
            int level = skillDic[skill.Name];

            // 가져왔는데 레벨이 0이다 -> 삭제한 스킬
            if (level == 0)
            {
                // 등록을 위한 false 반환
                return false;
            }
            // 스킬이 이미 최대 레벨에 도달했다
            else if (level >= skill.MaxLevel)
            {
                // 등록할 행동을 안하기 위한 true 반환
                return true;
            }

            // 레벨을 올려주는 로직
            level = (level >= skill.SkillLevel) ? level + 1 : skill.SkillLevel;
            skillDic[skill.Name] = level;
            skill.SkillLevel = level;

            Debug.Log($"<color=white>{skill.name} 스킬 {level}로 레벨업!</color>");
            // 등록하는 행동을 안하기 위한 true 반환
            return true;
        }
        // 스킬 등록이 안되어 있다 -> 한번도 장착을 안한 스킬 -> 등록을 위한 false 반환
        return false;
    }
}
