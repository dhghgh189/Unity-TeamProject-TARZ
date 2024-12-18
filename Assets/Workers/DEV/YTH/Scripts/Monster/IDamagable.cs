using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage);

    // Damage Over Time : 도트뎀 (장판기)
    // 장판기 위에서는 일정 시간마다 데미지가 계속 들어옴
    /*IEnumerator TakeDOTRoutine(int damage);

    WaitForSeconds attackDelay = new(2f);
    public IEnumerator TakeDOTRoutine(int damage)
    {
        while (true)
        {
            damagable.TakeDamage(damage);
            yield return attackDelay;
        }
    }*/

}
