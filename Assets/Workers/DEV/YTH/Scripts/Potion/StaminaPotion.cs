using UnityEngine;
using Zenject;

public class StaminaPotion : MonoBehaviour, IPotion
{
    [Inject]
    private PlayerController player;

    [SerializeField] float value;

    public void Use()
    {
        player.InfStamina(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use();
            Destroy(gameObject);
        }
    }
}
