using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public string Name;
    public string Description;
    public float Price;

    public virtual void Use(PlayerController player) { }
}
