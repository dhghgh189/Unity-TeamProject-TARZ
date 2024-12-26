using TMPro;
using UnityEngine;
using Zenject;

public class UI_StatModel : MonoBehaviour
{
    [Inject] StatModel statModel;
    [SerializeField] private TMP_Text statText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text mpText;
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text chipText;
    [SerializeField] private TMP_Text blackChipText;
    private void Awake()
    {
        statModel.OnStatChange += StatModel_OnStatChange;
        statModel.OnCurHpChange += StatModel_OnCurHpChange;
        statModel.OnCurMpChange += StatModel_OnCurMpChange;
        statModel.OnCurStaminaChange += StatModel_OnCurStaminaChange;
        statModel.OnChipChange += StatModel_OnChipChange;
        statModel.OnBlackChipChange += StatModel_OnBlackChipChange;

        hpText.text = $"체력: {statModel.CurrentHp}/{statModel.MaxHp}";
        mpText.text = $"마나: {statModel.CurrentMp}";
        staminaText.text = $"스테미나: {statModel.CurrentStamina}/{statModel.MaxStamina}";
        chipText.text = $"데이터 칩: {statModel.Chip}";
        blackChipText.text = $"블랙 데이터 칩: {statModel.BlackChip}";

        // UI 갱신을 위한 의미있는 함수
        statModel.SetAbility(AdditionAbility.AllPowerPer, 0);
    }

    private void StatModel_OnCurHpChange(float currentHp)
    {
        hpText.text = $"체력: {currentHp}/{statModel.MaxHp}";
    }

    private void StatModel_OnCurMpChange(float curreentMp)
    {
        mpText.text = $"마나: {curreentMp}";
    }

    private void StatModel_OnCurStaminaChange(float currentStamina)
    {
        staminaText.text = $"스테미나: {currentStamina}/{statModel.MaxStamina}";
    }

    private void StatModel_OnChipChange(float chip)
    {
        chipText.text = $"데이터 칩: {chip}";
    }

    private void StatModel_OnBlackChipChange(float blackChip)
    {
        blackChipText.text = $"블랙 데이터 칩: {blackChip}";
    }

    private void StatModel_OnStatChange()
    {
        statText.text = string.Empty;
        for (int i = 0; i < (int)AdditionAbility.Size; i++)
        {
            float value = statModel.GetAbility((AdditionAbility)i);
            if (value > 0)
            {
                statText.text += $"{((AdditionAbility)i).ToDescription()} : {value}\n";
            }
        }

        // UI 갱신을 위한
        statModel.CurrentHp += 0;
        statModel.CurrentMp += 0;
        statModel.ChangeStamina(0);
    }
}
