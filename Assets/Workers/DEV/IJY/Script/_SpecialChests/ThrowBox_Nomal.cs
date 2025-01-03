using UnityEngine;

public class ThrowBox_Nomal : SpecialThrowOBJ_Base
{
    [Header("중형 상자")]
    private LayerMask NomalBoxLayer;
    [SerializeField] private float NomalBoxDamage;


    void Start() => Init();

    void Init()
    {
        // 임시적 데미지 수치 설정
        NomalBoxDamage = 10f;

        NomalBoxLayer = LayerMask.GetMask("Monster");
    }

    //====================================================================


    private void OnCollisionEnter(Collision collision)
    {
        if (isThrowing)
        {
            ThrowingBox(collision.gameObject);
        }
    }

    private void ThrowingBox(GameObject OBJ)
    {
        if (OBJ.layer == NomalBoxLayer)
        {
            OBJ.GetComponent<IDamagable>().TakeDamage(NomalBoxDamage);
        }
        Destroy(this.gameObject, 1.5f);
    }
}
