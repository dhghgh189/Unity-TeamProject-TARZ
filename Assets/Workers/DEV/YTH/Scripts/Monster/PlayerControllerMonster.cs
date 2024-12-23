using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMonster : MonoBehaviour, IDamagable
{
    [SerializeField] float curHp;
    public float CurHp { get { return curHp; } set { curHp = value; } }
    [SerializeField] float maxHp = 100;

    private void Start()
    {
        curHp = maxHp;
    }

    private void Update()
    {
        
    }
    public void TakeDamage(float damage)
    {
       curHp -= damage;
    }
}
