using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using Zenject;

public class CheatUI : MonoBehaviour
{
    [Inject] StatModel model;
    [Inject] Inventory inventory;

    [SerializeField] PlayerController player;
    
    [SerializeField] GameObject dropGear;

    [Header("드롭다운")]
    [SerializeField] TMP_Dropdown statDropdown;
    [SerializeField] TMP_Dropdown partsDropdown;
    [SerializeField] TMP_Dropdown tierDropdown;
    [SerializeField] TMP_Dropdown ability01Dropdown;
    [SerializeField] TMP_Dropdown ability02Dropdown;

    [Header("인풋필드")]
    [SerializeField] TMP_InputField statInputField;
    [SerializeField] TMP_InputField ability01InputField;
    [SerializeField] TMP_InputField ability02InputField;


    private List<string> dropdownList;

    private void Awake()
    {
        Init_StatDropdown();
        Init_PartsDropdown();
        Init_AbilityDropdown();
    }

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

    public void MujeokMode(bool isOn)
    {
        Debug.Log(isOn);
        CheatManager.isMujeok = isOn;
    }

    public void ChipPlease()
    {
        model.Chip += 1000000f;
        model.BlackChip += 1000000f;
    }

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

    public void CreateGear()
    {
        Vector3 randomPos = player.transform.position + Random.insideUnitSphere * 5f;
        randomPos.y = 0;
        Instantiate(dropGear, randomPos, Quaternion.identity)
            .GetComponent<DropGear>().SetDropItem(Part.신발, 1, true, true);
    }

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
}
