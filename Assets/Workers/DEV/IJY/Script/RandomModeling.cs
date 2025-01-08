using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModeling : MonoBehaviour
{
    public List<Mesh> ThrowObjectMeshs = new();


    public Mesh SetRandom(List<Mesh> list)
    {
        int path = Random.Range(0, list.Count - 1);
        return list[path];
    }
}