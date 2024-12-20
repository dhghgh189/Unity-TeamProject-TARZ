using TMPro;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class UI_StatModel : MonoBehaviour
{
    [Inject] StatModel statModel;
    [SerializeField] private TMP_Text statText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text mpText;
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text chipText;
    [SerializeField] private TMP_Text atackPowerText;
    private void Awake()
    {
        statModel.OnStatChange += StatModel_OnStatChange;
        statModel.OnCurHpChange += StatModel_OnCurHpChange;
        statModel.OnCurMpChange += StatModel_OnCurMpChange;
        statModel.OnCurStaminaChange += StatModel_OnCurStaminaChange;
        statModel.OnChipChange += StatModel_OnChipChange;

        hpText.text = $"체력: {statModel.CurrentHp}/{statModel.MaxHp}";
        mpText.text = $"마나: {statModel.CurrentMp}";
        staminaText.text = $"스테미나: {statModel.CurrentStamina}/{statModel.MaxStamina}";
        chipText.text = $"데이터 칩: {statModel.Chip}";
    }

    private void StatModel_OnCurHpChange(float cureentHp)
    {
        hpText.text = $"체력: {cureentHp}/{statModel.MaxHp}";
    }

    private void StatModel_OnCurMpChange(float cureentMp)
    {
        mpText.text = $"마나: {cureentMp}";
    }

    private void StatModel_OnCurStaminaChange(float cureentStamina)
    {
        staminaText.text = $"스테미나: {cureentStamina}/{statModel.MaxStamina}";
    }

    private void StatModel_OnChipChange(float chip)
    {
        chipText.text = $"데이터 칩: {chip}";
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
    }
}
