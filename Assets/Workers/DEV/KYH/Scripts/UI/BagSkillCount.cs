using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BagSkillCount : MonoBehaviour
{
    [Inject] BagSkillManager bagSkill;

    [SerializeField] private Image bagSkill_01;
    [SerializeField] private Image bagSkill_02;
    [SerializeField] private float bagSkill01MaxValue;
    [SerializeField] private float bagSkill02MaxValue;
    /*[SerializeField] private float bagSkill01CurrentValue;
    [SerializeField] private float bagSkill02CurrentValue;*/

    private void Awake()
    {
        if (bagSkill.SkillArray[0] == null) return;
        bagSkill01MaxValue = bagSkill.SkillArray[0].MaxGauge;

        if (bagSkill.SkillArray[1] == null) return;
        bagSkill02MaxValue = bagSkill.SkillArray[1].MaxGauge;

        bagSkill.OnChargeEvent.AddListener(OnSkill_01Clear);
        bagSkill.OnChargeEvent.AddListener(OnSkill_02Clear);
    }

    private void Start()
    {
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
            bagSkill_01.fillAmount = bagSkill.SkillArray[0].CurGauge / bagSkill01MaxValue;
        }
    }

    private void UpdateSkill_02Gauge()
    {
        if (bagSkill_02 != null)
        {
            bagSkill_02.fillAmount = bagSkill.SkillArray[1].CurGauge / bagSkill02MaxValue;
        }
    }
}
