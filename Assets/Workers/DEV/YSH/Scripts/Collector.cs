using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        ThrowObject tobj = other.gameObject.GetComponent<ThrowObject>();
        if (tobj != null
            && tobj.IsCollected == false
            && player.Attack.ObjectCount < player.Attack.MaxObjectCount)
        {
            //Debug.Log($"Get Throw Object! : {tobj.name}");
            tobj.Get(player);
        }

        Buff buff = other.GetComponent<Buff>();
        if (buff != null)
        {
            buff.Use(player);
            Destroy(buff.gameObject);
        }
    }
}
