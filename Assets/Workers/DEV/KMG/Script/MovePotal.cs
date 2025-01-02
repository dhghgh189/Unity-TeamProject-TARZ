using UnityEngine;
using Zenject;

public class MovePotal : MonoBehaviour
{
    private Transform player;
    [SerializeField] Transform targetTransform;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include).transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = targetTransform.position;
    }

    public void SetTarget(Transform transform)
    {
        targetTransform = transform;
    }
}
