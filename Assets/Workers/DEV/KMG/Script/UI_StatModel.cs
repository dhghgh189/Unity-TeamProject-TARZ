using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class UI_StatModel : MonoBehaviour
{
    [Inject] StatModel statModel;
    private TMP_Text statText;
    private void Awake()
    {
        statModel.OnStatChange += StatModel_OnStatChange;
        statText = GetComponentInChildren<TMP_Text>();
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
