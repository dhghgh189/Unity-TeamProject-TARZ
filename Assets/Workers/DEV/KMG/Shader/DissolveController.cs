using System.Collections;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField] float DissolveTime;
    private Renderer renderers;
    private float dissolveFloat = -1;
    private void Awake()
    {
        renderers = GetComponentInChildren<Renderer>();
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
            foreach (var item in renderers.materials)
            {
                item.SetFloat("_Dissolve_Float", dissolveFloat);
            }
            dissolveFloat += 2 * (1 / DissolveTime) * Time.deltaTime;
            yield return null;
        }
    }
}
