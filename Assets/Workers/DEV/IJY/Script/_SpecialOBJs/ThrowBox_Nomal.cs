using UnityEngine;
using Zenject;

public class ThrowBox_Nomal : SpecialThrowOBJ_Base
{
    [Inject] private ObjectPool_other pool;
    public ThrowBox_Nomal() => box_type = Box_Type.Nomal;

    [Header("중형 상자")]
    [SerializeField] private float NomalBoxDamage;
    [SerializeField] private int Nomal_DropCount;
    [SerializeField] private float Nomal_DropSpred;
    private LayerMask NomalBoxLayer;


    void Start() => Init();

    void Init()
    {
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
            DropBlackChips(0.0f);
        }
        else
        {
            isDestroy = true;
            DropBlackChips(3.0f);
        }
    }

    void DropBlackChips(float DestroyTime)
    {
        Vector3 position = transform.position;

        while (Nomal_DropCount >= 1)
        {
            Nomal_DropCount--;
            position.x += Nomal_DropSpred * Random.value - Nomal_DropSpred / 2;
            position.z += Nomal_DropSpred * Random.value - Nomal_DropSpred / 2;
            if (position.y <= 0f) position.y = 0.1f;

            pool.DropChipItem(Random.Range(1, 50), position);
        }

        Destroy(this.gameObject, DestroyTime);
    }
}
