using UnityEngine;

public class DropChip : MonoBehaviour
{
    [SerializeField] float amount;
    [SerializeField] bool blackChip;
    [SerializeField] Material ChipMaterial;
    [SerializeField] Material blackChipMaterial;

    private void Awake()
    {
        if (blackChip)
            transform.GetComponentInChildren<Renderer>().material = blackChipMaterial;
        else
            transform.GetComponentInChildren<Renderer>().material = ChipMaterial;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Player")) return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player == null)
        {
            Debug.Log("DropChip Exception!");
            return;
        }

        if (blackChip)
            player.Stat.BlackChip += amount;
        else
            player.Stat.Chip += amount;

        gameObject.SetActive(false);
    }

    public void SetDropChip(float amount, bool blackChip)
    {
        this.amount = amount;
        this.blackChip = blackChip;

        if (blackChip)
            transform.GetComponentInChildren<Renderer>().material = blackChipMaterial;
        else
            transform.GetComponentInChildren<Renderer>().material = ChipMaterial;
    }
}
