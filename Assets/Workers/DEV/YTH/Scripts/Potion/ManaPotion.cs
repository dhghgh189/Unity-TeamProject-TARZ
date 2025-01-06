using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ManaPotion : MonoBehaviour, IPotion
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
        player.Stat.CurrentMp += player.Stat.MaxMp / 4;
    }
}
