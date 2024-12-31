using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    Coroutine loadingRoutine;

    public void StartLoading(Define.SceneType sceneType)
    {
        gameObject.SetActive(true);
        loadingRoutine = StartCoroutine(LoadingRoutine(sceneType));
    }

    IEnumerator LoadingRoutine(Define.SceneType sceneType)
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync((int)sceneType);

        oper.allowSceneActivation = false;

        float time = 0f;
        while (time < 3f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        oper.allowSceneActivation = true;
    }
}
