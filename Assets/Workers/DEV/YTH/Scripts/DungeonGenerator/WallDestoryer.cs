using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestoryer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
