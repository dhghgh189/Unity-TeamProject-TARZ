using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestClearPanel : MonoBehaviour
{
    [SerializeField] Button okayButton;

    private void OnEnable()
    {
        okayButton.Select();
    }
}
