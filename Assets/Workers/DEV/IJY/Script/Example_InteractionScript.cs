using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_InteractionScript : Base_InteractionOBJ
{
    public override void Activate()
    {
        Debug.Log($"{gameObject.name} : 말을 걸었다");
    }
}
