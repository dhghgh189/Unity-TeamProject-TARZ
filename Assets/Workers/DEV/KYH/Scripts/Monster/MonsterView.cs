using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MonsterView : MonoBehaviour
{
    [Inject] MonsterData monsterData;

    [SerializeField] private Slider hpView;

    private void Awake()
    {
        hpView.maxValue = monsterData.MaxHp;
        hpView.value = monsterData.CurHp;
    }

    public void OnChangeMonsterHP()
    {
        hpView.value = monsterData.CurHp;
        if (monsterData.CurHp >= 0)
        {
            hpView.value = 0;
        }
    }
}
