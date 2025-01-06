using UnityEngine;

public class ThrowBox_Nomal : SpecialThrowOBJ_Base
{
    public ThrowBox_Nomal() => box_type = Box_Type.Nomal;

    [Header("중형 상자")]
    private LayerMask NomalBoxLayer;
    [SerializeField] private float NomalBoxDamage;


    void Start() => Init();

    void Init()
    {
        // 임시적 데미지 수치 설정
        NomalBoxDamage = 10f;

        NomalBoxLayer = LayerMask.NameToLayer("Monster");
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
        if (isDestroy) return;

        if (OBJ.layer == NomalBoxLayer)
        {
            OBJ.GetComponent<IDamagable>().TakeDamage(NomalBoxDamage);
            Destroy(this.gameObject);
        }
        else
        {
            isDestroy = true;
            Destroy(this.gameObject, 3.0f);
        }
    }
}
