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

    /// <summary>
    /// 보스 몬스터의 HP값 변동 이벤트
    /// </summary>
    /// <param name="curHP"></param>
    /// <param name="maxHP"></param>
    public void OnChangeMonsterHP(float curHP, float maxHP)
    {
        hpFill.value = curHP;
        if (hpFill.value < 0) hpFill.value = 0;
    }

    /// <summary>
    /// 보스 몬스터의 이름 및 체력 정보를 연동
    /// </summary>
    /// <param name="data"></param>
    public void SetInfo(MonsterData data)
    {
        Debug.Log(data.name);
        nameText.text = data.name;
        hpFill.maxValue = data.MaxHp;
        hpFill.value = data.CurHp;

        data.pooledObject.OnDamage += OnChangeMonsterHP;
        pool = data.pooledObject;
    }

    // 해당 객체를 가진 몬스터가 사망 처리됐을 때 체력값 변동 이벤트 구독 해제
    private void OnDestroy()
    {
        pool.OnDamage -= OnChangeMonsterHP;
    }
}
