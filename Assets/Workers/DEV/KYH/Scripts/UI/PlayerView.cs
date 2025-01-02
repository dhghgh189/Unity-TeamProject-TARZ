using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [Inject] StatModel statModel;
    [Inject] PlayerController player;

    private PlayerAttack attack;

    [Header("플레이어 정보")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpImage;
    [SerializeField] private Slider mpSlider;
    [SerializeField] private Image mpImage;

    [Header("쓰레기 오브젝트")]
    [SerializeField] private TMP_Text numberingText;

    private void Start()
    {
        // 플레이어의 PlayerAttack 불러오기
        attack = player.Attack;

        // 슬라이더의 최대값을 각 스탯의 최대값으로 설정
        hpSlider.maxValue = statModel.MaxHp;
        mpSlider.maxValue = statModel.MaxMp;

        // 슬라이더의 조절 값을 각 스탯의 현재 값으로 설정
        hpSlider.value = statModel.CurrentHp;
        mpSlider.value = statModel.CurrentMp;

        // 플레이어의 각 스탯 변동 이벤트 구독
        statModel.OnCurHpChange += Player_OnCurHPChanged;
        statModel.OnCurMpChange += Player_OnCurMPChanged;
        statModel.OnStatChange += Player_OnTObjectChanged;
        attack.OnChangedStack += Player_OnTObjectChanged;

        // 이벤트 실행
        Player_OnCurHPChanged(statModel.MaxHp);
        Player_OnCurMPChanged(0);
        Player_OnTObjectChanged();
    }

    /// <summary>
    /// 플레이어 체력 값 변동
    /// </summary>
    /// <param name="curHP"></param>
    public void Player_OnCurHPChanged(float curHP)
    {
        //Debug.Log($"Hp Change : {curHP}");
        hpSlider.value = curHP;
        hpImage.enabled = curHP > 0;
    }

    /// <summary>
    /// 플레이어 마나 값 변동
    /// </summary>
    /// <param name="curMP"></param>
    public void Player_OnCurMPChanged(float curMP)
    {
        mpSlider.value = curMP;
        mpImage.enabled = curMP > 0;
    }

    /// <summary>
    /// 플레이어 투척 오브젝트 개수 변동
    /// </summary>
    public void Player_OnTObjectChanged()
    {
        numberingText.text = $"{attack.ObjectCount} / {attack.MaxObjectCount}";
    }

    /// <summary>
    /// 플레이어 오브젝트 파괴 시 이벤트 구독 해제
    /// </summary>
    private void OnDestroy()
    {
        statModel.OnCurHpChange -= Player_OnCurHPChanged;
        statModel.OnCurMpChange -= Player_OnCurMPChanged;
        statModel.OnStatChange -= Player_OnTObjectChanged;
        attack.OnChangedStack -= Player_OnTObjectChanged;
    }
}
