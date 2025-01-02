using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    Coroutine loadingRoutine;

    /// <summary>
    /// 로딩 과정 시작
    /// </summary>
    /// <param name="sceneType"></param>
    public void StartLoading(Define.SceneType sceneType)
    {
        gameObject.SetActive(true);
        loadingRoutine = StartCoroutine(LoadingRoutine(sceneType));
    }

    /// <summary>
    /// 로딩 과정 코루틴
    /// </summary>
    /// <param name="sceneType"></param>
    /// <returns></returns>
    IEnumerator LoadingRoutine(Define.SceneType sceneType)
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        AsyncOperation oper = SceneManager.LoadSceneAsync((int)sceneType);

        oper.allowSceneActivation = false;

        float time = 0f;
        while (time < 3f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        oper.allowSceneActivation = true;

        yield return Util.GetDelay(0.2f);
        gameObject.SetActive(false);

        //while (true)
        //{
        //    if (currentScene == SceneManager.GetActiveScene().buildIndex)
        //        yield return null;

        //    gameObject.SetActive(false);
        //    yield break;
        //}
    }
}
