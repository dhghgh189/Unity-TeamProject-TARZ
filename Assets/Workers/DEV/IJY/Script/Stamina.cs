using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Stamina : MonoBehaviour
{
    private float _waitTime;
    private float _lastStamina;
    private bool _IsChangeStam;

    [SerializeField] public bool IsGetPostion; // 추후 스테미나 회복 포션 획득 시 활성화 예정
    [Inject][SerializeField] private StatModel stat;
    [SerializeField] private Slider gauge_Stamina;

    private void Start()
    {
        stat.OnMaxStaminaChange += SliderMaxValueChange;
        stat.OnCurStaminaChange += OnSaveStamina;
        OnSaveStamina(stat.CurrentStamina);
        StartCoroutine(StaminaRoutine());
        _IsChangeStam = false;
        SliderMaxValueChange(stat.MaxStamina);
        gauge_Stamina.value = stat.MaxStamina;
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
                    stat.ChangeStamina(stat.StaminarEgeneration * Time.deltaTime, true);
                }
            }
            yield return null;
        }
    }
    private void SliderMaxValueChange(float maxStamina)
    {
        gauge_Stamina.maxValue = maxStamina;
    }
}
