using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;
public enum AdditionAbility
{
    [Description("공격력%")] AllPowerPer,
    [Description("기본 공격력%")] DefaultPowerPer,
    [Description("스킬 공격력%")] SkillPowerPer,
    [Description("속성 공격력%")] ElementalPowerPer,
    [Description("크리티컬 공격력%")] CriticalDamage,
    [Description("크리티컬 확률%")] Critical,
    [Description("최대 생명력%")] MaxHpPer,
    [Description("최대 스테미나%")] MaxStaminaPer,
    [Description("스테미나 재생속도%")] StaminarEgeneration,
    [Description("공격시 마나 회복력%")] MpGain,
    [Description("이동속도%")] MoveSpeedPer,
    [Description("최대 보유 오브젝트")] MaxObject,
    [Description("데이터 칩 획득량%")] ChipGetAmount,
    Size
}

public class StatModel : MonoBehaviour
{
    [Inject] SaveData saveData;
    [Header("기본 능력치")]

    [SerializeField] float maxHp;
    public float MaxHp { get => maxHp + (0.01f * maxHp * GetAbility(AdditionAbility.MaxHpPer)); set { maxHp = value; OnMaxHpChange?.Invoke(value); } }

    [SerializeField] float maxStamina;
    public float MaxStamina { get => maxStamina + (0.01f * maxStamina * GetAbility(AdditionAbility.MaxStaminaPer)); set { maxStamina = value; OnMaxStaminaChange?.Invoke(value); } }

    [SerializeField] float allPower;
    public float AllPower { get => allPower + (0.01f * allPower * GetAbility(AdditionAbility.AllPowerPer)); set { allPower = value; OnAllActackPowerChange?.Invoke(value); } }

    public float DefaultPower { get => AllPower + (0.01f * allPower * GetAbility(AdditionAbility.DefaultPowerPer)); private set { } }
    public float SkillPower { get => AllPower + (0.01f * allPower * GetAbility(AdditionAbility.SkillPowerPer)); private set { } }
    public float ElementalPower { get => AllPower + (0.01f * allPower * GetAbility(AdditionAbility.ElementalPowerPer)); private set { } }
    [Header("실시간 능력치")]

    [SerializeField] float currentHp;
    public float CurrentHp { get => currentHp; set { currentHp = value; OnCurHpChange?.Invoke(value); } }

    [SerializeField] float currentMp;
    public float CurrentMp { get => currentMp; set { currentMp = value; OnCurMpChange?.Invoke(value); } }

    [SerializeField] float currentStamina;
    public float CurrentStamina { get => currentStamina; set { currentStamina = value; OnCurStaminaChange?.Invoke(value); } }

    [SerializeField] float chip;
    public float Chip { get => chip; set { chip = value; OnChipChange?.Invoke(value); } }

    [SerializeField] float blackChip;
    public float BlackChip { get => blackChip; set { blackChip = value; OnBlackChipChange?.Invoke(value); } }

    [Header("추가 능력치")] // 아이템으로 상승하는 능력치 편의상 배열로 만들었음
    [SerializeField] float[] additionAbility = new float[(int)AdditionAbility.Size];
    public float GetAbility(AdditionAbility ability)
    {
        return additionAbility[(int)ability];
    }
    public void SetAbility(AdditionAbility ability, float value)
    {
        additionAbility[(int)ability] += value;
        OnStatChange.Invoke();
    }

    // 최대 체력 변경
    public event Action<float> OnMaxHpChange;
    // 최대 스테미나 변경
    public event Action<float> OnMaxStaminaChange;
    // 공격력 변경
    public event Action<float> OnAllActackPowerChange;
    // 현재 체력 변경
    public event Action<float> OnCurHpChange;
    // 현재 마나 변경
    public event Action<float> OnCurMpChange;
    // 현재 스테미나 변경
    public event Action<float> OnCurStaminaChange;
    // 데이터 칩 변경
    public event Action<float> OnChipChange;
    // 블랙 데이터 칩 변경
    public event Action<float> OnBlackChipChange;
    // 능력치 변경
    public event Action OnStatChange;

    private void Start()
    {
        if (saveData.StatSaveData == null) return;
        MaxHp = saveData.StatSaveData.maxHp;
        MaxStamina = saveData.StatSaveData.maxStamina;
        AllPower = saveData.StatSaveData.allPower;
        CurrentHp = saveData.StatSaveData.currentHp;
        CurrentMp = saveData.StatSaveData.currentMp;
        CurrentStamina = saveData.StatSaveData.currentStamina;
        Chip = saveData.StatSaveData.chip;
        BlackChip = saveData.StatSaveData.blackChip;
        for (int i = 0; i < (int)AdditionAbility.Size; i++) 
        {
            additionAbility[i] = saveData.StatSaveData.additionAbility[i];
        }
        SetAbility(AdditionAbility.AllPowerPer, 0);
    }
}
