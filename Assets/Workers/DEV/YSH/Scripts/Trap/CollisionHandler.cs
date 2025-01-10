using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private Collider coll;
    public Collider Coll => coll;

    public event Action<Collision> OnCollisionEntered;
    public event Action<Collision> OnCollisionStayed;

    public event Action<Collider> OnTriggerEntered;
    public event Action<Collider> OnTriggerStayed;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        OnCollisionEntered?.Invoke(other);
    }

    private void OnCollisionStay(Collision other)
    {
        OnCollisionStayed?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayed?.Invoke(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(other);
    }
}
