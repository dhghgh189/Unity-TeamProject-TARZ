using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    public bool IsEmptyRoom()
    {
        return Physics.OverlapSphere(transform.position, 1f, layerMask).Length == 0;
    }
}
