using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModeling : MonoBehaviour
{
    public List<GameObject> ThrowObjectOBJs = new();


    public void SetRandom(GameObject obj, List<GameObject> list)
    {
        int path = Random.Range(0, list.Count - 1);
        obj.GetComponent<MeshFilter>().sharedMesh = list[path].GetComponent<MeshFilter>().sharedMesh;
        obj.GetComponent<MeshRenderer>().sharedMaterial = list[path].GetComponent<MeshRenderer>().sharedMaterial;

        if (obj.transform.rotation != new Quaternion(0, 0, 0, 0))
        {
            obj.transform.rotation = list[path].transform.rotation;
        }
    }
}