using UnityEngine;
using Zenject;

public class MovePotal : MonoBehaviour
{
    [SerializeField] Vector3 targetPos;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        other.transform.position = targetPos;
    }

    public void SetTarget(Vector3 pos)
    {
        targetPos = pos;
    }
}
