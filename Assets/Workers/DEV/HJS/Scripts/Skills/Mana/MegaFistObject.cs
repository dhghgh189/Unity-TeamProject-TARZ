using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 거대 주먹 오브젝트에 부착하는 스크립트
/// </summary>
public class MegaFistObject : MonoBehaviour
{
    [Header("Init")]
    [SerializeField] float damage;          // 데미지
    [SerializeField] BoxCollider coll;      // 충돌을 확인하는 콜라이더
    [Header("Alpha")]
    [SerializeField] Material handMaterial;
    [SerializeField] Material bodyMaterial;
    [SerializeField] Material boostMaterial;
    [Range(0, 1)]
    [SerializeField] float alphaValue;
    public float AlphaValue { get => alphaValue; set { alphaValue = value; ChangeAlpha();  } }

    public float Damage { get => damage; set { damage = value; } }

    private void Start()
    {
       //  handMaterial.SetFloat("_SplitValue", 1.8f);
       //  bodyMaterial.SetFloat("_SplitValue", 1.2f);
       //  boostMaterial.SetFloat("_SplitValue", 0.5f);
    }

    private void ChangeAlpha()
    {
        handMaterial.SetFloat("_AlphaValue", alphaValue);
        bodyMaterial.SetFloat("_AlphaValue", alphaValue);
        boostMaterial.SetFloat("_AlphaValue", alphaValue);
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
