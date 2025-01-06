using UnityEngine;
using Zenject;

public class StaminaPotion : MonoBehaviour, IPotion
{
    [Inject]
    private PlayerController player;


    [SerializeField] float value; // 스태미너 무한 유지 시간

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
