using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded -= ResetDic;
        SceneManager.sceneLoaded += ResetDic;
    }

    private void ResetDic(Scene arg0, LoadSceneMode arg1)
    {
        particleDic = new();
    }

    Dictionary<string, Queue<GameObject>> particleDic = new();
    public void ParticlePlay(string particleName, float lifeTime, Vector3 pos, Quaternion rot, Transform pallowingTransform = null)
    {
        GameObject particlePrefab;
        ParticleSystem[] _particleSystem;
        List<ParticleSystem> subParticle = new();

        if (particleDic.ContainsKey(particleName) && particleDic[particleName].Count > 0)
        {
            particlePrefab = particleDic[particleName].Dequeue();
            particlePrefab.transform.rotation = rot;
            particlePrefab.transform.position = pos;
            particlePrefab.transform.parent = pallowingTransform;
            _particleSystem = particlePrefab.GetComponentsInChildren<ParticleSystem>(true);
        }
        else
        {
            particlePrefab = Resources.Load<GameObject>($"Managed/Effect/{particleName}");
            if (!particlePrefab)
            {
                Debug.LogWarning("Resources에서 없는 파티클을 호출");
                return;
            }

            particlePrefab.SetActive(false);

            particlePrefab = Instantiate(particlePrefab, pos, rot, pallowingTransform);
            _particleSystem = particlePrefab.GetComponentsInChildren<ParticleSystem>(true);
        }
        foreach (var item in _particleSystem)
        {
            var main = item.main;

            main.playOnAwake = true;

            main.duration = lifeTime;

            main.stopAction = ParticleSystemStopAction.Disable;

            if (item.subEmitters.subEmittersCount > 0)
            {
                for (int i = 0; i < item.subEmitters.subEmittersCount; i++)
                {
                    subParticle.Add(item.subEmitters.GetSubEmitterSystem(i));
                }
            }
        }
        foreach (var item in subParticle)
        {
            var main = item.main;
            main.stopAction = ParticleSystemStopAction.None;
        }
        foreach (var item in _particleSystem)
        {
            item.gameObject.SetActive(true);
        }
        particlePrefab.SetActive(true);

        StartCoroutine(ReturnPool(particleName, particlePrefab, lifeTime));
    }

    IEnumerator ReturnPool(string key, GameObject particleObject, float lifeTime)
    {
        yield return Util.GetDelay(lifeTime);

        if (!particleObject)
            yield break;

        particleObject.SetActive(false);

        if (particleDic.ContainsKey(key))
        {
            particleDic[key].Enqueue(particleObject);
        }
        else
        {
            particleDic.Add(key, new());
            particleDic[key].Enqueue(particleObject);
        }
    }
}