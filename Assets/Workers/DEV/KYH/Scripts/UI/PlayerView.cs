using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [Inject] StatModel statModel;
    [Inject] PlayerAttack playerAttack;

    [Header("플레이어 정보")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpImage;
    [SerializeField] private Slider mpSlider;
    [SerializeField] private Image mpImage;

    [Header("쓰레기 오브젝트")]
    [SerializeField] private TMP_Text numberingText;

    private void Start()
    {
        hpSlider.maxValue = statModel.MaxHp;
        mpSlider.maxValue = statModel.MaxMp;

        hpSlider.value = statModel.CurrentHp;
        mpSlider.value = statModel.CurrentMp;

        statModel.OnCurHpChange += Player_OnCurHPChanged;
        statModel.OnCurMpChange += Player_OnCurMPChanged;
        statModel.OnStatChange += Player_OnTObjectChanged;
        playerAttack.OnChangedStack += Player_OnTObjectChanged;

        Player_OnCurHPChanged(statModel.MaxHp);
        Player_OnCurMPChanged(0);
        Player_OnTObjectChanged();
    }

    public void Player_OnCurHPChanged(float curHP)
    {
        Debug.Log($"Hp Change : {curHP}");
        hpSlider.value = curHP;
        hpImage.enabled = curHP > 0;
    }

    public void Player_OnCurMPChanged(float curMP)
    {
        mpSlider.value = curMP;
        mpImage.enabled = curMP > 0;
    }

    public void Player_OnTObjectChanged()
    {
        numberingText.text = $"{playerAttack.ObjectCount} / {playerAttack.MaxObjectCount}";
    }
}
