using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState<T>
{
    protected T owner;
    public EState type { get; protected set; }

    public virtual void OnEnter() { /*Debug.Log($"{GetType()} OnEnter");*/ }
    public virtual void OnUpdate() { /*Debug.Log($"{GetType()} OnUpdate");*/ }
    public virtual void OnFixedUpdate() { /*Debug.Log($"{GetType()} OnFixedUpdate");*/ }
    public virtual void OnExit() { /*Debug.Log($"{GetType()} OnExit");*/ }
}
