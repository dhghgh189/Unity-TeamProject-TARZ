using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPGauge : MonoBehaviour
{
    private PooledObject pool;

    private Slider hpFill;
    private TMP_Text nameText;

    private void Awake()
    {
        hpFill = GetComponent<Slider>();
        nameText = GetComponentInChildren<TMP_Text>();
    }

    public void OnChangeMonsterHP(float curHP, float maxHP)
    {
        hpFill.value = curHP;
        if (hpFill.value < 0) hpFill.value = 0;
    }

    public void SetInfo(MonsterData data)
    {
        nameText.text = data.name;
        hpFill.maxValue = data.MaxHp;
        hpFill.value = data.CurHp;

        data.pooledObject.OnDamage += OnChangeMonsterHP;
        pool = data.pooledObject;
    }

    private void OnDestroy()
    {
        pool.OnDamage -= OnChangeMonsterHP;
    }
}
