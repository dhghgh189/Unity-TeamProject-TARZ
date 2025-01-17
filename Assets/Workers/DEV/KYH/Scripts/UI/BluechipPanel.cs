using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BluechipPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillLevel;
    [SerializeField] private TMP_Text skillDescription;
    [SerializeField] private Image skillIcon;

    /// <summary>
    /// 블루칩 스킬 초기화
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="des"></param>
    /// <param name="icon"></param>
    public void InitBluechipSkill(string name, string level, string des, Sprite icon)
    {
        SoundManager.PlaySFX(SoundManager.SoundData_UI.OnUI);
        skillName.text = name;
        skillLevel.text = level;
        skillDescription.text = des;
        skillIcon.sprite = icon;
    }
}
