using UnityEngine;

/// <summary>
/// 거대 주먹 오브젝트에 부착하는 스크립트
/// </summary>
public class MegaFistObject : MonoBehaviour
{
    [SerializeField] float damage;          // 데미지
    [SerializeField] BoxCollider coll;      // 충돌을 확인하는 콜라이더

    public float Damage { get => damage; set { damage = value; } }

    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
    }

    public void Move() => coll.enabled = true;      // 나올때는 충돌 작동하기

    public void Return() => coll.enabled = false;   // 돌아갈 때는 충돌 작동 끄기

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable != null) { damagable.TakeDamage(Damage); }
        }
    }

}
