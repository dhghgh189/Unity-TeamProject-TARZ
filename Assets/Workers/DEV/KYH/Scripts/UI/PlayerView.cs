using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [Inject] StatModel statModel;
    //[Inject] PlayerAttack playerAttack;

    [Header("플레이어 정보")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpImage;
    [SerializeField] private Slider mpSlider;
    [SerializeField] private Image mpImage;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Image staminaImage;

    [Header("쓰레기 오브젝트")]
    [SerializeField] private TMP_Text numberingText;

    private void Awake()
    {
        hpSlider.maxValue = statModel.MaxHp;
        mpSlider.maxValue = statModel.MaxMp;
        staminaSlider.maxValue = statModel.MaxStamina;

        hpSlider.value = statModel.CurrentHp;
        mpSlider.value = statModel.CurrentMp;
        staminaSlider.value = statModel.CurrentStamina;

        statModel.OnCurHpChange += Player_OnCurHPChanged;
        statModel.OnCurMpChange += Player_OnCurMPChanged;
        statModel.OnCurStaminaChange += Player_OnCurStaminaChanged;
        //playerAttack.OnChangedStack += Player_OnTObjectChanged;
    }

    public void Player_OnCurHPChanged(float curHP)
    {
        hpSlider.value = curHP;
        hpImage.enabled = curHP > 0;
    }

    public void Player_OnCurMPChanged(float curMP)
    {
        mpSlider.value = curMP;
        mpImage.enabled = curMP > 0;
    }

    public void Player_OnCurStaminaChanged(float curStamina)
    {
        staminaSlider.value = curStamina;
        staminaImage.enabled = curStamina > 0;
    }

    public void Player_OnTObjectChanged(int count)
    {
        //numberingText.text = $"{playerAttack.ObjectCount} / {playerAttack.MaxObjectCount}";
    }
}
