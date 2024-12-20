using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        ThrowObject tobj = other.gameObject.GetComponent<ThrowObject>();
        if (tobj != null && tobj.IsCollected == false)
        {
            //Debug.Log($"Get Throw Object! : {tobj.name}");
            tobj.Get(player);
        }
    }
}
