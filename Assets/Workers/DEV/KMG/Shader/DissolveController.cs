using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField] float DissolveTime;
    private List<Material> materials = new List<Material>();
    public float ReturnDissolveTime { get { return DissolveTime; } }

    private Renderer renderers;
    private float dissolveFloat = -1;

    private void Awake()
    {
        foreach (var render in GetComponentsInChildren<Renderer>())
        {
            materials.AddRange(render.materials);
        }
    }

    [ContextMenu("Dissolve")]
    public void StartDissolve()
    {
        StartCoroutine(Dissolve());
    }
    IEnumerator Dissolve()
    {
        while (dissolveFloat < 1)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve_Float", dissolveFloat);

                dissolveFloat += 2 * (1 / DissolveTime) * Time.deltaTime;
                yield return null;
            }
        }
    }
}
