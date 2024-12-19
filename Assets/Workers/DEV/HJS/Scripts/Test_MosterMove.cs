using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_MosterMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    void Update()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    }
}
