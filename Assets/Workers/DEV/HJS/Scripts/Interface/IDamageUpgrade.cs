using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageUpgrade
{
    public float IncreaseDamage { get; set; }
    public void UpgradeDamage(in float curDamage, out float Resultdamage);

}
