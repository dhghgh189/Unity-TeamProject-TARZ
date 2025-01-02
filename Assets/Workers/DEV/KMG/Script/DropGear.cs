using UnityEngine;
using Zenject;

public class DropGear : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] Part part;
    [SerializeField] int tier;
    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player") || !inventory.GetGear(part, tier)) return;
        gameObject.SetActive(false);
    }
    public void SetDropItem(Part part, int tier, bool isRandomPart = false, bool isRandomTier = false)
    {
        this.part = isRandomPart ? (Part)Random.Range(0, (int)Part.Size) : part;
        this.tier = isRandomTier ? Random.Range(1, 4) : tier;
    }
}
