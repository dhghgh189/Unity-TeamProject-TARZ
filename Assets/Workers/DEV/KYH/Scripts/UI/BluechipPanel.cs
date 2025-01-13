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

    public void InitBluechipSkill(string name, string level, string des, Sprite icon)
    {
        skillName.text = name;
        skillLevel.text = level;
        skillDescription.text = des;
        skillIcon.sprite = icon;
    }
}
