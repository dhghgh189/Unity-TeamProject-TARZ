using ModestTree;
using UnityEngine;

/// <summary>
/// 던지는 물체를 마인으로 변경해주는 스크립트
/// </summary>
public class ThrowObjectConvertMine : MonoBehaviour, IEnable, IChange
{
    [Header("Info")]
    [SerializeField] new string name = "ThrowObjectConvertMine";    // 스킬을 식별하는 고유 식별자
    [SerializeField] bool enable;                                   // 스킬의 활성화 여부
    [SerializeField] MeshRenderer render;                           // 해당 오브젝트의 Mesh Renderer
    [SerializeField] MeshFilter meshFilter;                         // 해당 오브젝트의 Mesh Filter
    [SerializeField] float damage;                                  // 해당 오브젝트의 데미지 
    [Header("Change Mesh")]
    [SerializeField] Mesh mesh;                                     // 변경할 Mesh
    [SerializeField] Material material;                             // 변경할 Material
    [SerializeField] string layer;                                  // 변경할 Layer

    public bool Enable { get => enable; set => enable = value; }
    public string Name { get => name; set => name = value; }
    public float MineDamage => damage;

    /// <summary>
    /// 변경을 요청하는 함수
    /// </summary>
    public void Change()
    {
        // 각 설정한 값이 있을 경우에만 변경
        if (mesh is not null) meshFilter.mesh = mesh;
        if (material is not null) render.material = material;
        if (!layer.IsEmpty()) gameObject.layer = LayerMask.NameToLayer(layer);
    }

    private void Awake()
    {
        render = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }


}
