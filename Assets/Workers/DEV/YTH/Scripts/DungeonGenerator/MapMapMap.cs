using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMapMap : MonoBehaviour
{
    [SerializeField] int roomCount;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject wallDestroyerPrefab;

    private Vector3[] createDir = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    void Start()
    {
        StartCoroutine(MapCreater());
    }
    IEnumerator MapCreater()
    {
        Vector3 createPos = Vector3.zero;
        for (int i = 0; i < roomCount; i++)
        {
            Instantiate(roomPrefab, createPos, Quaternion.identity);
            wallDestroyerPrefab.transform.position = createPos + (Vector3.up * 7);
            int random = Random.Range(0, 4);
            createPos += createDir[random] * 25f;
            wallDestroyerPrefab.transform.position = createPos + (Vector3.up * 7);
            createPos += createDir[random] * 25f;
            yield return null;
        }
    }
}
