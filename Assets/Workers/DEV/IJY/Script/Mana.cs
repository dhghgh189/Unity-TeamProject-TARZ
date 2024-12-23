using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class Mana : MonoBehaviour
{
    [Inject][SerializeField] private StatModel stat;
    [SerializeField] private Slider gauge_Mana;

    private void Start()
    {
        StartCoroutine(ManaRoutine());
    }

    IEnumerator ManaRoutine()
    {
        while (true)
        {

            yield return null;
        }
    }
}
