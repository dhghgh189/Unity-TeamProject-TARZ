using UnityEngine;
using Zenject;

public class ObjectPotion : MonoBehaviour, IPotion
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
       
    }
}
