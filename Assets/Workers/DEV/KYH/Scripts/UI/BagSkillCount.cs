using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagSkillCount : MonoBehaviour
{
    [SerializeField] private Image empty;
    [SerializeField] private Image fill;

    public void OnChangeDot()
    {
        empty.gameObject.SetActive(false);
        fill.gameObject.SetActive(true);
    }
}
