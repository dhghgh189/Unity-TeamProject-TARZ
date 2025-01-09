using UnityEngine;
using Zenject;

public class MovePotal : MonoBehaviour
{
    private Transform player;
    [SerializeField] Vector3 targetPos;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include).transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = targetPos;
    }

    public void SetTarget(Vector3 pos)
    {
        targetPos = pos;
    }
}
