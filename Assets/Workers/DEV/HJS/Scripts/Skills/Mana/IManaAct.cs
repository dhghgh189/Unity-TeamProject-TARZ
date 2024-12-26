using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManaAct
{
    public  void OnAction();
    public bool OnCollisionAction(Collision other);
}
