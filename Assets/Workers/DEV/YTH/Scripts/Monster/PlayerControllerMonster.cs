using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMonster : MonoBehaviour, IDamagable
{
    [SerializeField] int curHp;
    public int CurHp { get { return curHp; } set { curHp = value; } }
    [SerializeField] int maxHp = 100;

    private void Start()
    {
        curHp = maxHp;
    }

    private void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
       curHp -= damage;
    }
}
