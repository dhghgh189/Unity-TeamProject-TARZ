using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkFistObject : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    [SerializeField] GameObject fist;

    public void OnEffect() => ps.Play();
    
    public void OnFist() => fist.SetActive(true);

    public void OffEffect() => ps.Stop();
    public void OffFist() => fist.SetActive(false);
}
