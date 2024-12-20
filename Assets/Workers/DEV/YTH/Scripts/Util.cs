using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using UnityEngine;

public static partial class Util
{
    private static Dictionary<float, WaitForSeconds> _delayDic = new Dictionary<float, WaitForSeconds>();

    /// <summary>
    /// 코루틴 딜레이 가져오기
    /// </summary>
    /// 사용법 예시 :  yield return Util.GetDelay(0);
    public static WaitForSeconds GetDelay(this float time)
    {
        if (_delayDic.ContainsKey(time) == false)
        {
            _delayDic.Add(time, new WaitForSeconds(time));
        }
        return _delayDic[time];
    }

    /// <summary>
    /// 입력 값을 확률로 bool을 리턴하는 함수
    /// 사용법 예시 : if(IsRandom(50)), 50% 확률로 실행
    /// </summary>
    public static bool IsRandom(float probability)
    {
        if (UnityEngine.Random.Range(1, 101) > 100 - probability)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 열거형의 Description값을 리턴하는 함수
    /// 사용법 예시 : [Description("공격력%")]AllPowerPer, AdditionAbility.AllPowerPer.ToDescription();
    /// </summary>
    public static string ToDescription(this Enum source)
    {
        FieldInfo fi = source.GetType().GetField(source.ToString());
        var att = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
        if (att != null)
        {
            return att.Description;
        }
        else
        {
            return source.ToString();
        }
    }

}
