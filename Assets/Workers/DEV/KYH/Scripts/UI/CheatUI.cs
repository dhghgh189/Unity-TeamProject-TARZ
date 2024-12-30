using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CheatUI : MonoBehaviour
{
    [Inject] StatModel model;

    [SerializeField] PlayerController player;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject dropGear;

    private List<string> dropdownList;

    private void Awake()
    {
        // 현재 dropdown에 있는 모든 옵션을 제거
        dropdown.ClearOptions();

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
        dropdown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        dropdown.value = 0;
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
        switch ((CheatManager.StatType)dropdown.value)
        {
            case CheatManager.StatType.체력:
                model.CurrentHp = float.Parse(inputField.text);
                break;
            case CheatManager.StatType.최대체력:
                model.MaxHp = float.Parse(inputField.text);
                break;
            case CheatManager.StatType.스테미나:
                model.CurrentStamina = float.Parse(inputField.text);
                break;
            case CheatManager.StatType.최대스테미나:
                model.MaxStamina = float.Parse(inputField.text);
                break;
            case CheatManager.StatType.마나:
                model.CurrentMp = float.Parse(inputField.text);
                break;
            /*case CheatManager.StatType.최대마나:
                model.MaxMp = float.Parse(inputField.text);
                break;*/
            case CheatManager.StatType.이동속도:
                model.MoveSpeed = float.Parse(inputField.text);
                break;
            case CheatManager.StatType.공격력:
                model.SetAbility(AdditionAbility.AllPowerPer, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.기본공격력:
                model.SetAbility(AdditionAbility.DefaultPowerPer, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.스킬공격력:
                model.SetAbility(AdditionAbility.SkillPowerPer, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.속성공격력:
                model.SetAbility(AdditionAbility.ElementalPowerPer, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.공격시마나회복량:
                model.SetAbility(AdditionAbility.MpGain, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.스테미나재생속도:
                model.SetAbility(AdditionAbility.StaminarEgeneration, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.대쉬속도:
                model.DashSpeed = float.Parse(inputField.text);
                break;
            case CheatManager.StatType.대쉬스테미나소모량:
                model.DashStaminaAmount = float.Parse(inputField.text);
                break;
            case CheatManager.StatType.크리티컬공격력:
                model.SetAbility(AdditionAbility.CriticalDamage, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.크리티컬확률:
                model.SetAbility(AdditionAbility.Critical, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.최대보유오브젝트양:
                model.SetAbility(AdditionAbility.MaxObject, float.Parse(inputField.text));
                break;
            case CheatManager.StatType.데이터칩획득량:
                model.SetAbility(AdditionAbility.ChipGetAmount, float.Parse(inputField.text));
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
}
