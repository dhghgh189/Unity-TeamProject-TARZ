using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 실질적으로 장판역할을 해주는 스크립트
/// </summary>
public class FloorObject : MonoBehaviour
{
    // 파티클이 다 되면 삭제
    [SerializeField] ParticleSystem particle;
    [SerializeField] float duration;

    private void Start()
    {
        StartCoroutine(ParticleRoutine());
        if (duration == 0) duration = particle.main.duration;
    }

    private IEnumerator ParticleRoutine()
    {
        // yield return particle.main.duration;
        yield return Util.GetDelay(duration);

        Destroy(gameObject);
    }
}
