using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    /// <summary>
    /// Coroutine 앞에 ref 붙이기
    /// </summary>
    /// <param name="coroutine"></param>
    /// <param name="enumerator"></param>
    public void StartRoutine(ref Coroutine coroutine, IEnumerator enumerator) 
    {
        if (coroutine == null)
        {
            Debug.LogWarning("시작할겡2222222");
            coroutine = StartCoroutine(enumerator);
        }
        else
        {
           return;
        }
    }
}
