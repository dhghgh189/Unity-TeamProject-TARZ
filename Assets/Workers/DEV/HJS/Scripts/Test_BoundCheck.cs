using UnityEngine;

public class Test_BoundCheck : MonoBehaviour
{
    [SerializeField] Vector3 pos;
    [SerializeField] float distance;
    [SerializeField] float mar;
    [SerializeField] MeshRenderer render;
    [SerializeField] Bounds bounds;
    [SerializeField] Vector3 center;
    [SerializeField] Vector3 min;
    [SerializeField] Vector3 max;
    [SerializeField] Vector3 size;

    [ContextMenu("Bound")]
    public void CheckBound()
    {
        pos = transform.parent.position;

        if (render == null)
        {
            render = GetComponent<MeshRenderer>();
        }

        distance = Vector3.Distance(pos, transform.position);
        mar = Vector3.Magnitude(pos - transform.position);

        bounds = render.bounds;
        max = bounds.max;
        min = bounds.min;
        center = bounds.center;
        size = bounds.size;
    }

}
