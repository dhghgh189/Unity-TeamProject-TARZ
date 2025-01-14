using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
        // 로딩 동안 조작을 막는다.
        InputSystem.actions.FindActionMap("Player").Disable();
        
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

        Debug.Log(SceneManager.GetActiveScene().buildIndex);

        yield return Util.GetDelay(0.2f);
        gameObject.SetActive(false);

        // 로딩 종료되면 조작을 허용한다.
        InputSystem.actions.FindActionMap("Player").Enable();

        loadingRoutine = null;

        //while (true)
        //{
        //    if (currentScene == SceneManager.GetActiveScene().buildIndex)
        //        yield return null;

        //    gameObject.SetActive(false);
        //    yield break;
        //}
    }

    public bool IsUnLoading()
    {
        return loadingRoutine == null;
    }
}
