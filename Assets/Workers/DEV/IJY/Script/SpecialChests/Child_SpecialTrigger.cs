using UnityEngine;

public class Child_SpecialTrigger : MonoBehaviour
{
    private SpecialInteraction_Script SpecialOBJ;
    private SphereCollider col;
    private LayerMask playerLayer;
    public bool IsPlayerIn = false;

    void Start() => Init();

    void Init()
    {
        SpecialOBJ = GetComponentInParent<SpecialInteraction_Script>();
        playerLayer = LayerMask.NameToLayer("Player");

        col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = 3.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayerIn == true) return;

        if (other.gameObject.layer == playerLayer)
        {
            IsPlayerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayerIn == false) return;

        if (other.gameObject.layer == playerLayer)
        {
            IsPlayerIn = false;
        }
    }
}
