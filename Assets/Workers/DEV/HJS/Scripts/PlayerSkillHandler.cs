using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using static SkillEnum;

public class PlayerSkillHandler : MonoBehaviour
{
    [Header("Evnets")]
    private UnityEvent<GameObject, GameObject> onCollisionThrowObjectEvents;   // ThrowObject의 충돌 - OnCollision or OnTrigger
    private UnityEvent<GameObject, GameObject> onActionThrowObjectEvents;      // 기본 ThrowObject에서의 할일 - Enter
    public UnityEvent<string> onAddSkillEvents = new();
    private Dictionary<ActionTimingType, UnityEvent<GameObject, GameObject>[]> eventDic;

    public SkillContainer Container;

    private DrainManager drainManager;
    private PlayerMovement playerMovement;

    [Header("SkillList")]
    [SerializeField] public Dictionary<string, int> skillLevelDic = new();
    [SerializeField] public Dictionary<string, BaseSkillSO> skillDic = new();

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

        foreach (var value in eventDic.Values)
        {
            for (int i = 0; i < (int)EState.Length; i++)
            {
                value[i] = new UnityEvent<GameObject, GameObject>();
            }
        }

        //skillLevelDic = new Dictionary<string, int>();
        //skillDic = new Dictionary<string, BaseSkillSO>();
        // Skill 저장하기
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

        if (eventDic.TryGetValue(act, out var onResultEvent))
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
        if (LevelUp(skillName, setLevel))
        {
            onAddSkillEvents?.Invoke(skillName);
            return;
        }

        BaseSkillSO skill = Instantiate(Container.Skills.Where(x => x.Name == skillName).First());

        // 스킬리스트에 있으면 레벨 올려주기 <- 제거했다가 다시 추가했을 때
        if (skillLevelDic.ContainsKey(skill.Name))
        {
            Debug.Log("스킬 레벨업");
            int level = skillLevelDic[skill.Name];
            level += (setLevel >= skill.MaxLevel ? skill.MaxLevel : setLevel);
            skillLevelDic[skill.Name] = level;
            skill.SkillLevel = level;
            Debug.Log($"스킬 : {skill.Name} / {level}");
        }
        // 스킬리스트에 없으면 넣어두기
        else
        {
            skillLevelDic.Add(skill.Name, setLevel);
            skill.SkillLevel = (setLevel == 1) ? 1 : (setLevel >= skill.MaxLevel ? skill.MaxLevel : setLevel);
            Debug.Log("딕션에 추가!");
        }
        // 생성한 스킬 넣어두기
        skillDic[skillName] = skill;

        #region 액티브 스킬 넣기
        if ((skill.SkillType & SkillType.Act) != 0)
        {
            foreach (ActiveSkill actSkill in skill.ActiveSkills)
            {
                // 해당 스킬의 레벨에 따라 변경하기 위해 부모 설정
                actSkill.Parent = skill;
                // 레벨 변경의 대한 수치를 가져오기 위한 데이터베이스 넣기
                actSkill.SetModel(model);

                skill.onChangeLevel.AddListener(actSkill.UpdateLevel);

                if (actSkill.Target == SkillEnum.Target.Player)
                {
                    if (eventDic.TryGetValue(actSkill.ActTiming, out UnityEvent<GameObject, GameObject>[] onResultEvent))
                    {
                        onResultEvent[(int)actSkill.ConditionState].AddListener(actSkill.Use);
                    }
                }
                else if (actSkill.Target == SkillEnum.Target.ThrowObject)
                {
                    if (actSkill.ConditionType == ActConditionType.Start)
                        onActionThrowObjectEvents.AddListener(actSkill.Use);
                    else if (actSkill.ConditionType == ActConditionType.Collision)
                        onCollisionThrowObjectEvents.AddListener(actSkill.Use);
                }
            }
        }
        #endregion

        #region 패시브 스킬 넣기
        if ((skill.SkillType & SkillType.Etc) != 0)
        {
            foreach (PassiveSkill psivSkill in skill.PassiveSkills)
            {
                psivSkill.Parent = skill;
                psivSkill.StatModel = model;
                skill.onChangeLevel.AddListener(psivSkill.UpdateLevel);
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
                                Spec spec = new Spec();
                                spec.InteractionValues = psivSkill.GetToggleSetting.spec;
                                spec.statModel = model;
                                adapter.SetEnable(psivSkill.GetToggleSetting.Type, (skill.SkillLevel, spec));
                                break;
                        }
                        break;
                }
            }
        }
        #endregion

        // 디버그로 정보 보여주기
        Debug.Log($"Add Skill Name : {skill.Name}  / Skill Tier : {skill.SkillTier} / Skill Level : {skill.SkillLevel}");
        onAddSkillEvents?.Invoke(skillName);
        Debug.Log(onAddSkillEvents);
    }

    public List<BlueChipSaveData> SaveBlueChips()
    {
        List<BlueChipSaveData> blueChips = new();
        foreach (var item in skillLevelDic)
        {
            blueChips.Add(new BlueChipSaveData() { BlueChipName = item.Key, BlueChipLevel = item.Value });
        }
        return blueChips;
    }

    public void RemoveSkill(BaseSkillSO skill)
    {
        // 스킬이 없을 때 예외처리
        if (skill is null) return;

        #region 액티브 스킬 빼기
        if ((skill.SkillType & SkillType.Act) != 0)
        {
            foreach (ActiveSkill actSkill in skill.ActiveSkills)
            {
                // 해당 스킬의 레벨에 따라 변경하기 위해 부모 설정
                actSkill.Parent = skill;
                // 레벨 변경의 대한 수치를 가져오기 위한 데이터베이스 넣기
                actSkill.SetModel(model);

                skill.onChangeLevel.RemoveListener(actSkill.UpdateLevel);

                // 스킬의 사용 주체에 따라 실행
                if (actSkill.Target == SkillEnum.Target.Player)
                {
                    if (eventDic.TryGetValue(actSkill.ActTiming, out UnityEvent<GameObject, GameObject>[] te))
                    {
                        te[(int)actSkill.ConditionState].RemoveListener(actSkill.Use);
                    }
                }
                else if (actSkill.Target == SkillEnum.Target.ThrowObject)
                {
                    if (actSkill.ConditionType == ActConditionType.Start)
                        onActionThrowObjectEvents.RemoveListener(actSkill.Use);
                    else if (actSkill.ConditionType == ActConditionType.Collision)
                        onCollisionThrowObjectEvents.RemoveListener(actSkill.Use);
                }
            }
        }
        #endregion

        #region 패시브 스킬 빼기
        if ((skill.SkillType & SkillType.Etc) != 0)
        {
            foreach (PassiveSkill psivSkill in skill.PassiveSkills)
            {
                psivSkill.Parent = skill;
                skill.onChangeLevel.RemoveListener(psivSkill.UpdateLevel);

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
                                adapter.SetDisable(psivSkill.GetToggleSetting.Type);
                                break;
                        }
                        break;
                }
            }
        }
        #endregion

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

        foreach (var item in skillDic.Values)
        {
            if (item is not null)
            {
                RemoveSkill(item);
                Destroy(item);
            }
        }

        skillDic.Clear();
        skillLevelDic.Clear();
        eventDic.Clear();
    }

    public bool LevelUp(string skillName, int skillLevel = 1)
    {
        // 스킬에 등록이 되어있다면 -> 기존에 한번이라도 장착은 한 스킬
        if (skillLevelDic.ContainsKey(skillName))
        {
            // 해당 스킬의 레벨을 가져온다
            int level = skillLevelDic[skillName];

            // 가져왔는데 레벨이 0이다 -> 삭제한 스킬
            if (level == 0)
            {
                // 등록을 위한 false 반환
                return false;
            }

            // 스킬의 최대 레벨을 가져온다
            int maxLevel = skillDic[skillName].MaxLevel;

            // 스킬이 이미 최대 레벨에 도달했다
            if (level >= maxLevel)
            {
                // 등록할 행동을 안하기 위한 true 반환
                return true;
            }

            // 레벨을 올려주는 로직
            skillLevelDic[skillName] = (level >= skillLevel) ? level + 1 : skillLevel;  // 만약 현재 레벨이 더 높다 -> 1레벨 업, 추가하는 스킬이 더 높다 해당 스킬의 레벨로

            skillDic[skillName].SkillLevel = skillLevelDic[skillName];                  // 장착한 스킬의 레벨 최신화

            UpdatePassiveByLevel(skillDic[skillName], level);

            Debug.Log($"<color=white>{skillName} 스킬 {skillLevelDic[skillName]}로 레벨업!</color>");
            // 등록하는 행동을 안하기 위한 true 반환
            return true;
        }
        // 스킬 등록이 안되어 있다 -> 한번도 장착을 안한 스킬 -> 등록을 위한 false 반환
        return false;
    }

    // TODO: 해당 내용들을 StatModel에서 관리하기 -> Skill안에서만 해결하기로
    private void UpdatePassiveByLevel(BaseSkillSO skill, int level)
    {
        foreach (PassiveSkill psivSkill in skill.PassiveSkills)
        {
            if (psivSkill.GetPassiveType.Equals(PassiveType.Modify))
            {
                switch (psivSkill.GetModifySetting.ModifyType)
                {
                    case PassiveModifyType.DashSpeed:
                        model.DashSpeed -= psivSkill.GetModifySetting.Amount(level);
                        model.DashSpeed += psivSkill.GetModifySetting.Amount(skill.SkillLevel);
                        break;
                    case PassiveModifyType.DrainRadius:
                        drainManager.MaxRadius -= psivSkill.GetModifySetting.Amount(level);
                        drainManager.MaxRadius += psivSkill.GetModifySetting.Amount(skill.SkillLevel);
                        break;
                }
            }
            else if (psivSkill.GetPassiveType.Equals(PassiveType.Toggle))
            {
                var item = adapter.levelDic[psivSkill.GetToggleSetting.Type];
                item.Item1 = skillLevelDic[skill.Name] - 1;
                adapter.levelDic[psivSkill.GetToggleSetting.Type] = item;
                Debug.Log($"Toggle 업데이트 : {item.Item1} : {level - 1}");
            }
        }
    }
}
