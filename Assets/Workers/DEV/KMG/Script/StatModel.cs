using System;
using UnityEngine;
public enum AdditionAbility
{
    AllPowerPer, DefaultPowerPer, SkillPowerPer, ElementalPowerPer, CriticalDamage, Critical, MaxHpPer, MaxStaminaPer, StaminarEgeneration, MpGain, MoveSpeedPer, MaxObject, ChipGetAmount, Size
}

public class StatModel : MonoBehaviour
{
    [Header("기본 능력치")]
    [SerializeField] float maxHp;
    public float MaxHp { get => maxHp; private set { } }

    [SerializeField] float maxStamina;
    public float MaxStamina { get => maxStamina; private set { } }

    [SerializeField] float allPower;
    public float AllPower { get => allPower; private set { } }
    [Header("실시간 능력치")]

    [SerializeField] float currentHp;
    public float CurrentHp { get => currentHp; set { currentHp = value; OnCurHpChange.Invoke(value); } }

    [SerializeField] float currentMp;
    public float CurrentMp { get => currentMp; set { currentMp = value; OnCurMpChange.Invoke(value); } }

    [SerializeField] float currentStamina;
    public float CurrentStamina { get => currentStamina; set { currentStamina = value; OnCurStaminaChange.Invoke(value); } }

    [SerializeField] float chip;
    public float Chip { get => chip; set { chip = value; OnChipChange.Invoke(value); } }

    [SerializeField] float blackChip;
    public float BlackChip { get => blackChip; set { blackChip = value; OnBlackChipChange.Invoke(value); } }

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
}
