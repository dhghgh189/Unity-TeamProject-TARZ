using UnityEngine;

public class DropGear : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] Part part;
    [SerializeField] int tier;

    private float pValue;
    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player") || !inventory.GetGear(part, tier, pValue)) return;
        gameObject.SetActive(false);
    }
    public void SetDropItem(int tier, float pValue, bool isRandomTier = false)
    {
        part = (Part)Random.Range(0, (int)Part.Size);
        this.tier = isRandomTier ? Random.Range(1, 4) : tier;
        this.pValue = pValue;
    }
}
