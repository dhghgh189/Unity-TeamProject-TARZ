using UnityEngine;
using Zenject;

public class HpPotion : MonoBehaviour, IPotion
{
    [Inject]
    private PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use();
            Destroy(gameObject);
        }
    }

    public void Use()
    {
        player.Stat.CurrentHp += player.Stat.MaxHp / 3;
    }
}
