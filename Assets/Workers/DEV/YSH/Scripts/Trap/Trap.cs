using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool activateOnAwake = true;

    private bool isActive;
    public bool IsActive => isActive;

    public abstract void Activate();
    public abstract void Deactivate();

    private void Awake()
    {
        if (activateOnAwake)
            Activate();
    }
}
