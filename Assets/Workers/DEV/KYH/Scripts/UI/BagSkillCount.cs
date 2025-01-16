using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BagSkillCount : MonoBehaviour
{
    [Inject] BagSkillManager bagSkill;

    [SerializeField] private Sprite defaultIcon;

    [SerializeField] private Image bagSkill_01;
    [SerializeField] private Image bagSkill_02;
    [SerializeField] private Image bagSkillIcon_01;
    [SerializeField] private Image bagSkillIcon_02;
    [SerializeField] private float bagSkill01MaxValue;
    [SerializeField] private float bagSkill02MaxValue;
    /*[SerializeField] private float bagSkill01CurrentValue;
    [SerializeField] private float bagSkill02CurrentValue;*/
    
    private void Awake()
    {
        bagSkillIcon_01.sprite = defaultIcon;
        bagSkillIcon_02.sprite = defaultIcon;

        if (bagSkill.SkillArray[0] == null) return;
        bagSkill01MaxValue = bagSkill.SkillArray[0].MaxGauge;
        

        if (bagSkill.SkillArray[1] == null) return;
        bagSkill02MaxValue = bagSkill.SkillArray[1].MaxGauge;
        
    }

    private void Start()
    {
        bagSkill.OnUIUpdateEvent.AddListener(UpdateSkill_01Gauge);
        bagSkill.OnUIUpdateEvent.AddListener(UpdateSkill_02Gauge);

        if (bagSkill_01 != null)
        {
            bagSkill_01.fillAmount = 0;
        }

        if (bagSkill_02 != null)
        {
            bagSkill_02.fillAmount = 0;
        }
    }

    public void OnSkill_01Clear()
    {
        if (bagSkill.SkillArray[0].CurGauge < bagSkill01MaxValue)
        {
            bagSkill.SkillArray[0].CurGauge++;
            UpdateSkill_01Gauge();
        }
    }

    public void OnSkill_02Clear()
    {
        if (bagSkill.SkillArray[1].CurGauge < bagSkill02MaxValue)
        {
            bagSkill.SkillArray[1].CurGauge++;
            UpdateSkill_02Gauge();
        }
    }

    private void UpdateSkill_01Gauge()
    {
        if (bagSkill_01 != null)
        {
            Color color = bagSkill_01.GetComponent<Image>().color;

            if (bagSkill.SkillArray[0] == null) return;

            bagSkill01MaxValue = bagSkill.SkillArray[0].MaxGauge;
            bagSkill_01.fillAmount = bagSkill.SkillArray[0].CurGauge / bagSkill01MaxValue;
            Debug.Log($"setting1{bagSkill.SkillArray[0].CurGauge} / {bagSkill01MaxValue} => {bagSkill_01.fillAmount}");

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

            bagSkillIcon_01.sprite = bagSkill.SkillArray[0].icon;
        }
    }

    private void UpdateSkill_02Gauge()
    {
        if (bagSkill_02 != null)
        {
            Color color = bagSkill_02.GetComponent<Image>().color;

            if (bagSkill.SkillArray[1] == null) return;

            bagSkill02MaxValue = bagSkill.SkillArray[1].MaxGauge;
            bagSkill_02.fillAmount = bagSkill.SkillArray[1].CurGauge / bagSkill02MaxValue;
            Debug.Log($"setting2{bagSkill.SkillArray[1].CurGauge} / {bagSkill02MaxValue} => {bagSkill_02.fillAmount}");

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

            bagSkillIcon_02.sprite = bagSkill.SkillArray[1].icon;
        }
    }
}
