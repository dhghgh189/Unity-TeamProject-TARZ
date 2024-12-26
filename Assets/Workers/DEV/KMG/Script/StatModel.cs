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
    public float MaxHp { get => maxHp + (maxHp * GetAbility(AdditionAbility.MaxHpPer) * 0.01f); set { maxHp = value; OnMaxHpChange?.Invoke(MaxHp); } }

    [SerializeField] float maxStamina;
    public float MaxStamina { get => maxStamina + (maxStamina * GetAbility(AdditionAbility.MaxStaminaPer) * 0.01f); set { maxStamina = value; OnMaxStaminaChange?.Invoke(MaxStamina); } }

    [SerializeField] float maxMp;
    public float MaxMp { get => maxMp; private set { } }

    [SerializeField] float moveSpeed;
    public float MoveSpeed { get => moveSpeed + (moveSpeed * GetAbility(AdditionAbility.MoveSpeedPer) * 0.01f); set { moveSpeed = value; OnMoveSpeedChange?.Invoke(MoveSpeed); } }
    public float AllPowerPer { get => GetAbility(AdditionAbility.AllPowerPer); private set { } }
    public float DefaultPowerPer { get => 1 + ((AllPowerPer + (GetAbility(AdditionAbility.DefaultPowerPer))) * 0.01f); private set { } }
    public float SkillPowerPer { get => 1 + ((AllPowerPer + (GetAbility(AdditionAbility.SkillPowerPer))) * 0.01f); private set { } }
    public float ElementalPowerPer { get => 1 + ((AllPowerPer + (GetAbility(AdditionAbility.ElementalPowerPer))) * 0.01f); private set { } }

    public float[] MpAmount = new float[(int)EMpAmountType.Length];
    public float GetMpGain(EMpAmountType amountType)
    {
        return MpAmount[(int)amountType] * (1 + (GetAbility(AdditionAbility.MpGain) * 0.01f));
    }

    [SerializeField] float staminarEgeneration;
    public float StaminarEgeneration { get => staminarEgeneration * (1 + ((GetAbility(AdditionAbility.StaminarEgeneration)) * 0.01f)); private set { } }

    public float DashSpeed;

    public float DashStaminaAmount;

    // 스테미너가 소모되는 행동 진행 시 곱해줘야 하는 값
    public float StaminaCostRate = 1f;

    [Header("실시간 능력치")]

    [SerializeField] float currentHp;
    public float CurrentHp { 
        get => currentHp; 
        set 
        {
            currentHp = Mathf.Clamp(value, 0, MaxHp);
            OnCurHpChange?.Invoke(currentHp);
        } 
    }

    [SerializeField] float currentMp;
    public float CurrentMp { 
        get => currentMp; 
        set 
        {
            currentMp = Mathf.Clamp(value, 0, maxMp);
            OnCurMpChange?.Invoke(currentMp); 
        } 
    }

    [SerializeField] float currentStamina;
    public float CurrentStamina 
    { 
        get => currentStamina;
        set
        {
            currentStamina = Mathf.Clamp(value, 0, MaxStamina);
            OnCurStaminaChange?.Invoke(currentStamina);
        }
    }

    /// <summary>
    /// Stamina에 더하거나 뺄 값을 넘겨준다.
    /// </summary>
    /// <param name="stamina"></param>
    public void ChangeStamina(float stamina)
    {
        currentStamina += stamina * StaminaCostRate;
        currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
        OnCurStaminaChange?.Invoke(currentStamina);
    }

    [SerializeField] float chip;
    public float Chip 
    {
        get => chip;
        set 
        {
            chip = value; 
            OnChipChange?.Invoke(value); 
        }
    }

    [SerializeField] float blackChip;
    public float BlackChip 
    { 
        get => blackChip; 
        set 
        {
            blackChip = value; 
            OnBlackChipChange?.Invoke(value); 
        } 
    }

    [Header("추가 능력치")] // 아이템으로 상승하는 능력치 편의상 배열로 만들었음
    [SerializeField] float[] additionAbility = new float[(int)AdditionAbility.Size];
    public float GetAbility(AdditionAbility ability)
    {
        return additionAbility[(int)ability];
    }
    public void SetAbility(AdditionAbility ability, float value)
    {
        additionAbility[(int)ability] += value;
        OnStatChange?.Invoke();
        OnMaxHpChange?.Invoke(MaxHp);
        OnMaxStaminaChange?.Invoke(MaxStamina);
        OnMoveSpeedChange?.Invoke(MoveSpeed);
    }

    // 최대 체력 변경
    public event Action<float> OnMaxHpChange;
    // 최대 스테미나 변경
    public event Action<float> OnMaxStaminaChange;
    // 이동 속도 변경
    public event Action<float> OnMoveSpeedChange;
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
        CurrentHp = saveData.StatSaveData.currentHp;
        CurrentMp = saveData.StatSaveData.currentMp;
        CurrentStamina = saveData.StatSaveData.currentStamina;
        BlackChip = saveData.StatSaveData.blackChip;
        for (int i = 0; i < (int)AdditionAbility.Size; i++) 
        {
            additionAbility[i] = saveData.StatSaveData.additionAbility[i];
        }
        SetAbility(AdditionAbility.AllPowerPer, 0);
    }

    public void AllCheck()
    {
        OnMaxHpChange?.Invoke(MaxHp);
        OnMaxStaminaChange?.Invoke(MaxStamina);
        OnMoveSpeedChange?.Invoke(MoveSpeed);
        OnCurHpChange?.Invoke(currentHp);
        OnCurMpChange?.Invoke(currentMp);
        OnCurStaminaChange?.Invoke(currentStamina);
        OnChipChange?.Invoke(Chip);
        OnBlackChipChange?.Invoke(BlackChip);
        OnStatChange?.Invoke();
    }
}
