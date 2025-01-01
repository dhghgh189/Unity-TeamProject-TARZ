using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    public bool IsEmptyRoom()
    {
        return Physics.OverlapSphere(transform.position, 1f).Length == 0;
    }
}
