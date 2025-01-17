using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BagSkillCount : MonoBehaviour
{
    [Inject] BagSkillManager bagSkill;

    [SerializeField] private Sprite defaultIcon;    // 빈 아이콘

    [SerializeField] private Image bagSkill_01;
    [SerializeField] private Image bagSkill_02;
    [SerializeField] private Image bagSkillIcon_01;
    [SerializeField] private Image bagSkillIcon_02;
    [SerializeField] private float bagSkill01MaxValue;
    [SerializeField] private float bagSkill02MaxValue;
    
    private void Awake()
    {
        // 가방 스킬 장착 전 아이콘을 빈 아이콘으로 출력
        bagSkillIcon_01.sprite = defaultIcon;
        bagSkillIcon_02.sprite = defaultIcon;

        // 가방 스킬의 충전 값을 슬라이더에 동기화
        if (bagSkill.SkillArray[0] == null) return;
        bagSkill01MaxValue = bagSkill.SkillArray[0].MaxGauge;
        

        if (bagSkill.SkillArray[1] == null) return;
        bagSkill02MaxValue = bagSkill.SkillArray[1].MaxGauge;
        
    }

    private void Start()
    {
        // 슬라이더 UI 값 변경 이벤트 구독
        bagSkill.OnUIUpdateEvent.AddListener(UpdateSkill_01Gauge);
        bagSkill.OnUIUpdateEvent.AddListener(UpdateSkill_02Gauge);

        // 시작할 때 슬라이더의 값으 0으로 설정
        if (bagSkill_01 != null)
        {
            bagSkill_01.fillAmount = 0;
        }

        if (bagSkill_02 != null)
        {
            bagSkill_02.fillAmount = 0;
        }
    }

    /// <summary>
    /// 가방 스킬 1번 슬롯 초기화
    /// </summary>
    public void OnSkill_01Clear()
    {
        if (bagSkill.SkillArray[0].CurGauge < bagSkill01MaxValue)
        {
            bagSkill.SkillArray[0].CurGauge++;
            UpdateSkill_01Gauge();
        }
    }

    /// <summary>
    /// 가방 스킬 2번 슬롯 초기화
    /// </summary>
    public void OnSkill_02Clear()
    {
        if (bagSkill.SkillArray[1].CurGauge < bagSkill02MaxValue)
        {
            bagSkill.SkillArray[1].CurGauge++;
            UpdateSkill_02Gauge();
        }
    }

    /// <summary>
    /// 가방 스킬 1번 슬롯 참조 및 UI 업데이트
    /// </summary>
    private void UpdateSkill_01Gauge()
    {
        if (bagSkill_01 != null)
        {
            Color color = bagSkill_01.GetComponent<Image>().color;

            if (bagSkill.SkillArray[0] == null) return;

            // 게이지 슬라이더 값 연동
            bagSkill01MaxValue = bagSkill.SkillArray[0].MaxGauge;
            bagSkill_01.fillAmount = bagSkill.SkillArray[0].CurGauge / bagSkill01MaxValue;
            Debug.Log($"setting1{bagSkill.SkillArray[0].CurGauge} / {bagSkill01MaxValue} => {bagSkill_01.fillAmount}");

            // 게이지가 꽉 찼을 때, 투명도를 조절하여 충전 완료 상태 표시
            if (bagSkill_01.fillAmount == 1)
            {
                color.a = 1f;
                bagSkill_01.GetComponent<Image>().color = color;
            }
            else
            {
                color.a = 0.67f;
                bagSkill_01.GetComponent<Image>().color = color;
            }

            // 장착된 가방 스킬의 아이콘 출력
            bagSkillIcon_01.sprite = bagSkill.SkillArray[0].icon;
        }
    }

    /// <summary>
    /// 가방 스킬 2번 슬롯 참조 및 UI 업데이트
    /// </summary>
    private void UpdateSkill_02Gauge()
    {
        if (bagSkill_02 != null)
        {
            Color color = bagSkill_02.GetComponent<Image>().color;

            if (bagSkill.SkillArray[1] == null) return;

            // 게이지 슬라이더 값 연동
            bagSkill02MaxValue = bagSkill.SkillArray[1].MaxGauge;
            bagSkill_02.fillAmount = bagSkill.SkillArray[1].CurGauge / bagSkill02MaxValue;
            Debug.Log($"setting2{bagSkill.SkillArray[1].CurGauge} / {bagSkill02MaxValue} => {bagSkill_02.fillAmount}");

            // 게이지가 꽉 찼을 때, 투명도를 조절하여 충전 완료 상태 표시
            if (bagSkill_02.fillAmount == 1)
            {
                color.a = 1f;
                bagSkill_02.GetComponent<Image>().color = color;
            }
            else
            {
                color.a = 0.67f;
                bagSkill_02.GetComponent<Image>().color = color;
            }

            // 장착된 가방 스킬의 아이콘 출력
            bagSkillIcon_02.sprite = bagSkill.SkillArray[1].icon;
        }
    }
}
