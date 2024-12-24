using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectConvertMine : MonoBehaviour, IEnable, IChange
{
    [Header("Info")]
    [SerializeField] string name = "ThrowObjectConvertMine";
    [SerializeField] bool enable;
    [SerializeField] MeshRenderer render;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] private float damage;
    [Header("Change Mesh")]
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;
    [SerializeField] string layer;

    public bool Enable { get => enable; set => enable = value; }
    public string Name { get => name; set => name = value; }
    public float MineDamage => damage;

    public void Change()
    {
        if(mesh is not null) meshFilter.mesh = mesh;
        if(material is not null) render.material = material;
        if(!layer.IsEmpty())gameObject.layer = LayerMask.NameToLayer(layer);
    }

    private void Awake()
    {
        render = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }


}
