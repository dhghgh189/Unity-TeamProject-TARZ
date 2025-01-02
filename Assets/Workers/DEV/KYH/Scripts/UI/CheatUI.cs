using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using Zenject;

public class CheatUI : MonoBehaviour
{
    // Zenject로 주입한 클래스
    [Inject] StatModel model;
    [Inject] Inventory inventory;
    //[Inject] ChangeInput inputSystem;

    [SerializeField] PlayerController player;
    [SerializeField] SkillSpecDatabase skillData;
    [SerializeField] GameObject dropGear;
    [SerializeField] Toggle mujeokToggle;
    [SerializeField] GameObject cheatUIPanel;
    [SerializeField] ChangeInput inputManager;

    // 각 치트 옵션을 설정할 Dropdown UI
    [Header("드롭다운")]
    [SerializeField] TMP_Dropdown statDropdown;
    [SerializeField] TMP_Dropdown partsDropdown;
    [SerializeField] TMP_Dropdown tierDropdown;
    [SerializeField] TMP_Dropdown ability01Dropdown;
    [SerializeField] TMP_Dropdown ability02Dropdown;
    [SerializeField] TMP_Dropdown skillDropdown;
    [SerializeField] TMP_Dropdown levelDropdown;

    // 각 치트 옵션의 수치를 설정할 InputField UI
    [Header("인풋필드")]
    [SerializeField] TMP_InputField statInputField;
    [SerializeField] TMP_InputField ability01InputField;
    [SerializeField] TMP_InputField ability02InputField;

    // Dropdown 리스트
    private List<string> dropdownList;

    private void Awake()
    {
        // 모든 드롭다운을 초기화하는 과정
        Init_StatDropdown();
        Init_PartsDropdown();
        Init_AbilityDropdown();
        Init_skillDropdown();
    }

    private void Start()
    {
        if (cheatUIPanel.activeSelf)
        {
            inputManager.firstInput = mujeokToggle;
            inputManager.firstInput.Select();
        }
    }

    /// <summary>
    /// 스탯 치트 드롭다운 초기화
    /// </summary>
    private void Init_StatDropdown()
    {
        // 현재 dropdown에 있는 모든 옵션을 제거
        statDropdown.ClearOptions();

        dropdownList = new List<string>((int)CheatManager.StatType.Size);

        for (int i = 0; i < dropdownList.Capacity; i++)
        {
            dropdownList.Add(((CheatManager.StatType)i).ToString());
        }

        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        // arrayClass 배열에 있는 모든 문자열 데이터를 불러와서 optionList에 저장
        foreach (string str in dropdownList)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }

        // 위에서 생성한 optionList를 dropdown의 옵션 값에 추가
        statDropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        statDropdown.value = 0;
    }

    /// <summary>
    /// 아이템 파츠 치트 드롭다운 초기화
    /// </summary>
    private void Init_PartsDropdown()
    {
        // 현재 dropdown에 있는 모든 옵션을 제거
        partsDropdown.ClearOptions();

        dropdownList = new List<string>((int)Part.Size);

        for (int i = 0; i < dropdownList.Capacity; i++)
        {
            dropdownList.Add(((Part)i).ToString());
        }

        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        // arrayClass 배열에 있는 모든 문자열 데이터를 불러와서 optionList에 저장
        foreach (string str in dropdownList)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }

        // 위에서 생성한 optionList를 dropdown의 옵션 값에 추가
        partsDropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        partsDropdown.value = 0;
    }

    /// <summary>
    /// 능력치 치트 드롭다운 초기화
    /// </summary>
    private void Init_AbilityDropdown()
    {
        // 현재 dropdown에 있는 모든 옵션을 제거
        ability01Dropdown.ClearOptions();
        ability02Dropdown.ClearOptions();

        dropdownList = new List<string>((int)AdditionAbility.Size);

        for (int i = 0; i < dropdownList.Capacity; i++)
        {
            dropdownList.Add(((AdditionAbility)i).ToString());
        }

        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        // arrayClass 배열에 있는 모든 문자열 데이터를 불러와서 optionList에 저장
        foreach (string str in dropdownList)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }

        // 위에서 생성한 optionList를 dropdown의 옵션 값에 추가
        ability01Dropdown.AddOptions(optionList);
        ability02Dropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        ability01Dropdown.value = 0;
        ability02Dropdown.value = 0;
    }

    /// <summary>
    /// 스킬 치트 드롭다운 초기화
    /// </summary>
    private void Init_skillDropdown()
    {
        // 현재 dropdown에 있는 모든 옵션을 제거
        skillDropdown.ClearOptions();

        dropdownList = new List<string>(skillData.skillList.Count);

        for (int i = 0; i < skillData.skillList.Count; i++)
        {
            dropdownList.Add(skillData.skillList[i].Name);
        }

        // 새로운 옵션 설정을 위한 OptionData 생성
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        // arrayClass 배열에 있는 모든 문자열 데이터를 불러와서 optionList에 저장
        foreach (string str in dropdownList)
        {
            optionList.Add(new TMP_Dropdown.OptionData(str));
        }

        // 위에서 생성한 optionList를 dropdown의 옵션 값에 추가
        skillDropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        skillDropdown.value = 0;
    }

    /// <summary>
    /// 무적 상태 설정 치트
    /// </summary>
    /// <param name="isOn"></param>
    public void MujeokMode(bool isOn)
    {
        Debug.Log($"무적 상태 : {isOn}");
        CheatManager.isMujeok = isOn;
    }

    /// <summary>
    /// 칩 대량 제공 치트
    /// </summary>
    public void ChipPlease()
    {
        model.Chip += 1000000f;
        model.BlackChip += 1000000f;
    }

    /// <summary>
    /// 스탯 수치 설정 치트
    /// </summary>
    public void SetStat()
    {
        switch ((CheatManager.StatType)statDropdown.value)
        {
            case CheatManager.StatType.체력:
                model.CurrentHp = float.Parse(statInputField.text);
                break;
            case CheatManager.StatType.최대체력:
                model.MaxHp = float.Parse(statInputField.text);
                break;
            case CheatManager.StatType.스테미나:
                model.CurrentStamina = float.Parse(statInputField.text);
                break;
            case CheatManager.StatType.최대스테미나:
                model.MaxStamina = float.Parse(statInputField.text);
                break;
            case CheatManager.StatType.마나:
                model.CurrentMp = float.Parse(statInputField.text);
                break;
            /*case CheatManager.StatType.최대마나:
                model.MaxMp = float.Parse(inputField.text);
                break;*/
            case CheatManager.StatType.이동속도:
                model.MoveSpeed = float.Parse(statInputField.text);
                break;
            case CheatManager.StatType.공격력:
                model.SetAbility(AdditionAbility.AllPowerPer, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.기본공격력:
                model.SetAbility(AdditionAbility.DefaultPowerPer, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.스킬공격력:
                model.SetAbility(AdditionAbility.SkillPowerPer, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.속성공격력:
                model.SetAbility(AdditionAbility.ElementalPowerPer, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.공격시마나회복량:
                model.SetAbility(AdditionAbility.MpGain, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.스테미나재생속도:
                model.SetAbility(AdditionAbility.StaminarEgeneration, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.대쉬속도:
                model.DashSpeed = float.Parse(statInputField.text);
                break;
            case CheatManager.StatType.대쉬스테미나소모량:
                model.DashStaminaAmount = float.Parse(statInputField.text);
                break;
            case CheatManager.StatType.크리티컬공격력:
                model.SetAbility(AdditionAbility.CriticalDamage, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.크리티컬확률:
                model.SetAbility(AdditionAbility.Critical, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.최대보유오브젝트양:
                model.SetAbility(AdditionAbility.MaxObject, float.Parse(statInputField.text));
                break;
            case CheatManager.StatType.데이터칩획득량:
                model.SetAbility(AdditionAbility.ChipGetAmount, float.Parse(statInputField.text));
                break;
        }
    }

    /// <summary>
    /// 원하는 아이템 생성 치트
    /// </summary>
    public void CreateGear()
    {
        Vector3 randomPos = player.transform.position + Random.insideUnitSphere * 5f;
        randomPos.y = 0;
        Instantiate(dropGear, randomPos, Quaternion.identity)
            .GetComponent<DropGear>().SetDropItem(Part.신발, 1, true, true);
    }

    /// <summary>
    /// 원하는 아이템 획득 치트
    /// </summary>
    public void GetGear() 
    {
        Gear gear = ScriptableObject.CreateInstance<Gear>();
        gear.Part = (Part)partsDropdown.value;
        gear.Tier = tierDropdown.value + 1;

        GearAbility ability01 = new GearAbility()
        {
            ability = (AdditionAbility)ability01Dropdown.value,
            value = float.Parse(ability01InputField.text),
        };

        GearAbility ability02 = new GearAbility()
        {
            ability = (AdditionAbility)ability02Dropdown.value,
            value = float.Parse(ability02InputField.text),
        };

        gear.Abilities = new List<GearAbility>()
        {
            ability01, ability02
        };

        inventory.GetGear(gear);
    }

    /// <summary>
    /// 체력 회복 치트
    /// </summary>
    public void Heal()
    {
        model.CurrentHp = model.MaxHp;
    }

    /// <summary>
    /// 마나 무제한 치트
    /// </summary>
    /// <param name="isOn"></param>
    public void ManaInfinite(bool isOn)
    {
        Debug.Log($"마나 무한 : {isOn}");
        CheatManager.isManaInfinite = isOn;
    }

    /// <summary>
    /// 블루칩 스킬 설정 치트
    /// </summary>
    public void RandomSkill()
    {
        BaseSkillSO[] skill = skillData.ShowSkillArray();
        player.SkillHandler.AddSkill(skill[0].Name);
    }

    /// <summary>
    /// 블루칩 스킬 획득 치트
    /// </summary>
    public void GetSkill()
    {
        BaseSkillSO skill = Instantiate(skillData.skillList[skillDropdown.value]);
        skill.SkillLevel = levelDropdown.value + 1;
        player.SkillHandler.AddSkill(skill.Name, levelDropdown.value + 1);
    }
}
