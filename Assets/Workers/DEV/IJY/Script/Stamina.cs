using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Stamina : MonoBehaviour
{

    #region 임시적 사용 자료형 (추후 PlayerStat에 추가 요망)
    [Header("스테미나 수치 변동값")]
    [SerializeField] private float _plusStam;
    #endregion

    private float _waitTime;
    private float _lastStamina;
    private bool _IsChangeStam;

    [Inject][SerializeField] private StatModel stat;
    [SerializeField] private Slider gauge_Stamina;

    private void Start()
    {
        stat.OnCurStaminaChange += OnSaveStamina;
        OnSaveStamina(stat.CurrentStamina);
        StartCoroutine(StaminaRoutine());
        _IsChangeStam = false;
    }

    private void OnSaveStamina(float lastStamina)
    {
        if (lastStamina < _lastStamina)
        {
            _IsChangeStam = true;
            _waitTime = 1f;
        }
        _lastStamina = lastStamina;
        gauge_Stamina.value = _lastStamina;
    }

    IEnumerator StaminaRoutine()
    {
        // 현재 스테미나가 최대치 미만일 때, 스테미나 수치를 최대치 만큼 끌어올리도록 한다
        while (true)
        {
            if (stat.CurrentStamina <= 0.0f && _IsChangeStam == true)
            {
                _waitTime = 3.0f;
                _IsChangeStam = false;
            }
            if (_waitTime > 0.0f)
            {
                _waitTime -= Time.deltaTime;
                yield return null;
            }
            else
            {
                if (stat.CurrentStamina < stat.MaxStamina)
                {
                    stat.ChangeStamina(stat.StaminarEgeneration * Time.deltaTime);
                }
            }
            yield return null;
        }
    }
}
