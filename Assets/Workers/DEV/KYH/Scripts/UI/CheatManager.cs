using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class CheatManager
{
    public static bool isMujeok;
    public static bool isManaInfinite;

    public enum StatType
    {
        체력, 최대체력, 스테미나, 최대스테미나, 마나, 최대마나,
        이동속도, 공격력, 기본공격력, 스킬공격력, 속성공격력,
        공격시마나회복량, 스테미나재생속도, 대쉬속도, 대쉬스테미나소모량,
        크리티컬공격력, 크리티컬확률, 최대보유오브젝트양, 데이터칩획득량,
        Size
    }
}
