using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// QA 및 테스트 용도의 치트 기능 데이터 관리용 클래스
/// </summary>
public static class CheatManager
{
    public static bool isMujeok;            // 무적 상태 여부 bool 변수
    public static bool isManaInfinite;      // 마나 무한 상태 여부 bool 변수

    // 드롭다운에 표시할 스탯 종류
    public enum StatType
    {
        체력, 최대체력, 스테미나, 최대스테미나, 마나, 최대마나,
        이동속도, 공격력, 기본공격력, 스킬공격력, 속성공격력,
        공격시마나회복량, 스테미나재생속도, 대쉬속도, 대쉬스테미나소모량,
        크리티컬공격력, 크리티컬확률, 최대보유오브젝트양, 데이터칩획득량,
        Size
    }
}
