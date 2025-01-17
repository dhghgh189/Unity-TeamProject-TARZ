using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

public class RedchipPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillGauge;
    [SerializeField] private TMP_Text skillDescription;
    [SerializeField] private Image skillIcon;

    //[SerializeField] private LocalizedStringTable localTable;

    /// <summary>
    /// 가방 스킬 초기화 함수
    /// </summary>
    /// <param name="data"></param>
    public void InitRedchipSkill(BagSkillDataSO data)
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.OnUI);
        skillName.text = data.SkillName.ToString();
        skillGauge.text = $"{data.MaxGauge} / {data.UseAmount}";
        skillDescription.text = data.SkillDescription;
        skillIcon.sprite = data.SkillIcon;
    }
}
