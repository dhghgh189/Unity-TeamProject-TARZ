using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    #region 임시적 사용 자료형 (추후 PlayerStat에 추가 요망)
    [SerializeField] private float curStam;
    [SerializeField] private float minStam;
    [SerializeField] private float maxStam;

    [SerializeField] private float _plusStam;
    [SerializeField] private float _deductStam;
    [SerializeField] private float _reductionStam;

    [SerializeField] private bool _isZeroStam;
    #endregion

    [SerializeField] private Slider gauge_Stamina;


    private void Start() => _isZeroStam = false;

    IEnumerator StaminaRoutine()
    {
        curStam = gauge_Stamina.value;

        yield return null;
    }
}
