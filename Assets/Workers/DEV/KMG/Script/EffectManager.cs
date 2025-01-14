using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    Dictionary<string, Queue<GameObject>> particleDic = new();
    public void ParticlePlay(string particleName, float lifeTime, Vector3 pos, Quaternion rot, Transform pallowingTransform = null)
    {
        GameObject particlePrefab;
        ParticleSystem _particleSystem;

        if (particleDic.ContainsKey(particleName) && particleDic[particleName].Count > 0)
        {
            particlePrefab = particleDic[particleName].Dequeue();
            particlePrefab.transform.rotation = rot;
            particlePrefab.transform.position = pos;
            particlePrefab.transform.parent = pallowingTransform;
            _particleSystem = particlePrefab.GetComponent<ParticleSystem>();
        }
        else
        {
            particlePrefab = Resources.Load<GameObject>($"Effect/{particleName}");
            if (!particlePrefab)
            {
                Debug.LogWarning("Resources에서 없는 파티클을 호출");
                return;
            }

            particlePrefab.SetActive(false);

            _particleSystem = Instantiate(particlePrefab, pos, rot, pallowingTransform).GetComponent<ParticleSystem>();
        }

        var main = _particleSystem.main;

        main.playOnAwake = true;

        main.duration = lifeTime;

        main.stopAction = ParticleSystemStopAction.Disable;

        _particleSystem.gameObject.SetActive(true);

        StartCoroutine(ReturnPool(particleName, _particleSystem.gameObject, lifeTime));
    }

    IEnumerator ReturnPool(string key, GameObject particleObject, float lifeTime)
    {
        yield return Util.GetDelay(lifeTime);

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
