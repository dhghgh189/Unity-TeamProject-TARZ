using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// 강화 가능하게 해주는 기능
/// </summary>
public class ThrowObjectUpgrade : MonoBehaviour, IEnable, IDamageUpgrade
{
    [SerializeField] bool enable;
    [SerializeField] string name = "ThrowObjectUpgrade";
    [SerializeField] float increaseDamage;
    public bool Enable { get => enable; set => enable = value; }
    public string Name { get => name; set => name = value; }
    public float IncreaseDamage { get => increaseDamage; set => increaseDamage = value; }

    /// <summary>
    /// 데미지를 업그레이드 시켜주는 함수
    /// </summary>
    /// <param name="curDamage"></param>
    /// <param name="Resultdamage"></param>
    public void UpgradeDamage(in float curDamage, out float Resultdamage)
    {
        Resultdamage = curDamage * IncreaseDamage;
    }

}
