using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// 임시 선언한 공격 타입. 현재 Stern만 사용한다.
public enum TakeHitType
{
    Stern,
    Doto_Damage,
    Grab,
    Knock_Back, Knock_Up,
    Super_Knock_Back, Super_Knock_Up,
    Size
}

public interface IDamagable
{
    void TakeDamage(float damage);

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
