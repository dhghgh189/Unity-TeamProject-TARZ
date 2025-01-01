using UnityEngine;

public class MapMap : MonoBehaviour
{
    [SerializeField] int roomCount;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject tunnelPrefab;

    private float tunnelDistanceWidth;
    private float tunnelDistanceHeight;

    private Vector3[] TunnelDir = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    private float[] tunnelRotateY = { 0, 0, 90, 90 };
    private float[] tunnelDistance = new float[4];
    void Start()
    {
        tunnelDistanceWidth = roomPrefab.GetComponent<BoxCollider>().size.x * 0.5f + tunnelPrefab.GetComponent<BoxCollider>().size.x * 0.5f;
        tunnelDistanceHeight = roomPrefab.GetComponent<BoxCollider>().size.z * 0.5f + tunnelPrefab.GetComponent<BoxCollider>().size.z * 0.5f;
        tunnelDistance[0] = tunnelDistanceHeight;
        tunnelDistance[1] = tunnelDistanceHeight;
        tunnelDistance[2] = tunnelDistanceWidth;
        tunnelDistance[3] = tunnelDistanceWidth;
        CreateMap();
    }
    private void CreateMap()
    {
        Vector3 currentRoomPos = Vector3.zero;
        for (int i = 0; i < roomCount; i++)
        {
            Instantiate(roomPrefab, currentRoomPos, Quaternion.identity);
            int random = Random.Range(0, 4);
            currentRoomPos += TunnelDir[random] * tunnelDistance[random];
            Quaternion quaternion = Quaternion.identity;
            quaternion.eulerAngles = new Vector3(0, tunnelRotateY[random], 0);
            Instantiate(tunnelPrefab, currentRoomPos, quaternion);
            currentRoomPos += TunnelDir[random] * tunnelDistance[random];
        }
    }
}
