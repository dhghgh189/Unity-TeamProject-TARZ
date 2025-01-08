using TMPro;
using UnityEngine;
using Zenject;

public class UI_StatModel : MonoBehaviour
{
    [Inject] StatModel statModel;
    [SerializeField] private TMP_Text statText;
    [SerializeField] private TMP_Text maxHp;
    [SerializeField] private TMP_Text maxStamina;
    [SerializeField] private TMP_Text moveSpeed;
    private void Awake()
    {
        statModel.OnStatChange += StatModel_OnStatChange;
        statModel.OnMaxHpChange += StatModel_OnMaxHpChange;
        statModel.OnMaxStaminaChange += StatModel_OnMaxStaminaChange;
        statModel.OnMoveSpeedChange += StatModel_OnMoveSpeedChange;

        maxHp.text = $"체력\t\t{statModel.MaxHp}";
        maxStamina.text = $"스테미나\t{statModel.MaxStamina}";
        moveSpeed.text = $"이동속도\t{statModel.MoveSpeed}";

        // UI 갱신을 위한 의미있는 함수
        statModel.SetAbility(AdditionAbility.AllPowerPer, 0);
    }

    private void StatModel_OnMoveSpeedChange(float obj)
    {
        moveSpeed.text = $"이동속도\t{statModel.MoveSpeed}";
    }

    private void StatModel_OnMaxStaminaChange(float currentStamina)
    {
        maxStamina.text = $"스테미나\t{statModel.MaxStamina}";
    }
    private void StatModel_OnMaxHpChange(float currentHp)
    {
        maxHp.text = $"체력\t\t{statModel.MaxHp}";
    }

    private void StatModel_OnStatChange()
    {
        statText.text = string.Empty;
        for (int i = 0; i < (int)AdditionAbility.Size; i++)
        {
            float value = statModel.GetAbility((AdditionAbility)i);
            if (value > 0)
            {
                statText.text += $"{((AdditionAbility)i).ToDescription()}\t\t{value}\n";
            }
        }
    }

    private void OnDestroy()
    {
        statModel.OnStatChange -= StatModel_OnStatChange;
        statModel.OnMaxHpChange -= StatModel_OnMaxHpChange;
        statModel.OnMaxStaminaChange -= StatModel_OnMaxStaminaChange;
        statModel.OnMoveSpeedChange -= StatModel_OnMoveSpeedChange;
    }
}
