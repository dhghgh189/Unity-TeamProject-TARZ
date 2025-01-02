using UnityEngine;
using Zenject;

public class DropChip : MonoBehaviour
{
    [Inject] StatModel statModel;
    [SerializeField] float amount;
    [SerializeField] bool blackChip;
    [SerializeField] Material ChipMaterial;
    [SerializeField] Material blackChipMaterial;

    private void Awake()
    {
        if (blackChip)
            transform.GetComponent<Renderer>().material = blackChipMaterial;
        else
            transform.GetComponent<Renderer>().material = ChipMaterial;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player")) return;

        if (blackChip)
            statModel.BlackChip += amount;
        else
            statModel.Chip += amount;

        gameObject.SetActive(false);
    }

    public void SetDropChip(float amount, bool blackChip)
    {
        this.amount = amount;
        this.blackChip = blackChip;

        if (blackChip)
            transform.GetComponent<Renderer>().material = blackChipMaterial;
        else
            transform.GetComponent<Renderer>().material = ChipMaterial;
    }
}
