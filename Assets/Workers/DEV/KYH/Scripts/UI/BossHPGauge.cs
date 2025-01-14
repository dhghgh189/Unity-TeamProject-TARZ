using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPGauge : MonoBehaviour
{
    private PooledObject pool;

    [SerializeField]private Slider hpFill;
    [SerializeField] private TMP_Text nameText;

    public void OnChangeMonsterHP(float curHP, float maxHP)
    {
        hpFill.value = curHP;
        if (hpFill.value < 0) hpFill.value = 0;
    }

    public void SetInfo(MonsterData data)
    {
        Debug.Log(data.name);
        nameText.text = data.name;  // TODO : name 자리에 MonsterData의 name 변수 받아오기
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
