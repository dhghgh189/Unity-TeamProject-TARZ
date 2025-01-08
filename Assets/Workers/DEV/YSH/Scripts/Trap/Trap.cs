using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool activateOnAwake = true;

    protected bool isActive;
    public bool IsActive => isActive;

    public abstract void Activate();
    public abstract void Deactivate();

    protected virtual void Init() 
    {
        if (activateOnAwake)
            Activate();
    }

    private void Awake()
    {
        Init();
    }
}
