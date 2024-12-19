using System.Collections;
using System.Collections.Generic;
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
}
