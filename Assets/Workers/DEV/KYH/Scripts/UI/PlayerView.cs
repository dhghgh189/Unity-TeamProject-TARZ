using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [Inject] StatModel statModel;

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
        //mpSlider.maxValue = statModel.MaxMp;
        staminaSlider.maxValue = statModel.MaxStamina;

        hpSlider.value = statModel.CurrentHp;
        mpSlider.value = statModel.CurrentMp;
        staminaSlider.value = statModel.CurrentStamina;

        statModel.OnCurHpChange += Player_OnCurHPChanged;
        statModel.OnCurMpChange += Player_OnCurMPChanged;
        statModel.OnCurStaminaChange += Player_OnCurStaminaChanged;
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

    public void Player_OnGarbageChanged(float count)
    {
        // TODO : 플레이어가 가지고 있는 투척물 개수를 텍스트로 실시간 업데이트하여 출력
    }
}
