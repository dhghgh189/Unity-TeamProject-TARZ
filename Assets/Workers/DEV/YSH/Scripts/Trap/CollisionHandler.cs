using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private Collider coll;
    public Collider Coll => coll;

    public event Action<Collider> OnTriggered;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggered?.Invoke(other);
    }
}
