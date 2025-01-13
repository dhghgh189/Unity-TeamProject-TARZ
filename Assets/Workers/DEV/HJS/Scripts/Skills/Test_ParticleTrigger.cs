using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test_ParticleTrigger : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        // 파티클이 닿았을 때 가장 가까이 있는 녀석이 -> 닿은 친구
        int num = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
            
    }
}
