using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int roomCount;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject storePrefab;
    [SerializeField] GameObject wallDestroyer;
    [SerializeField] RoomChecker roomChecker;

    // 랜덤한 방향을 담을 배열
    private Vector3[] createDir = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    // 파괴자의 y좌표
    private Vector3 destroyerY = new Vector3(0, 7f, 0);

    // 가장 먼 방을 찾기 위한 변수
    Vector3 farDistancePos;
    Vector3 bossRoomDir;

    Vector3 maxZPos;
    Vector3 maxXPos;

    // 방을 생성할 위치
    Vector3 createPos;

    private int random;

    void Start()
    {
        StartCoroutine(MapCreater());
    }
    IEnumerator MapCreater()
    {
        for (int i = 0; i < roomCount; i++)
        {
            // 네 방향 중 비여있는 위치를 구해서 createPos에 넣음
            roomChecker.transform.position = createPos;
            if (!roomChecker.IsEmptyRoom())
            {
                i--;
                createPos += createDir[random] * 50f;
                wallDestroyer.transform.position = destroyerY + createPos - createDir[random] * 25f;
                continue;
            }

            Instantiate(roomPrefab, createPos, Quaternion.identity, transform);

            // 보스장 생성을 위한 가장 먼 방 체크
            FindFarRoomPos();

            // 랜덤 방향 지정
            random = Random.Range(0, 4);
            createPos += createDir[random] * 50f;


            // 진행 방향의 벽 제거
            if (i == roomCount - 1)
                break;
            yield return Util.GetDelay(0.05f);
            wallDestroyer.transform.position = destroyerY + createPos - createDir[random] * 25f;
        }

        SetBossRoomDir();
        Debug.Log(farDistancePos);
        // 보스룸 통로 생성
        for (int i = 0; i < 2; i++)
        {
            Instantiate(roomPrefab, farDistancePos, Quaternion.identity, transform);
            yield return Util.GetDelay(0.05f);
            wallDestroyer.transform.position = destroyerY + farDistancePos - bossRoomDir * 25f;
            farDistancePos += bossRoomDir * 50;
        }
        yield return Util.GetDelay(0.05f);
        Destroy(wallDestroyer.gameObject);
        Destroy(roomChecker.gameObject);
    }
    private void SetBossRoomDir()
    {
        while (true)
        {
            random = Random.Range(0, 4);
            bossRoomDir = createDir[random];
            farDistancePos += bossRoomDir * 50;
            roomChecker.transform.position = farDistancePos;
            if (roomChecker.IsEmptyRoom())
                break;
            farDistancePos -= bossRoomDir * 50;
        }
    }
    private void FindFarRoomPos()
    {
        if (farDistancePos.sqrMagnitude < createPos.sqrMagnitude)
            farDistancePos = createPos;

        if (Mathf.Abs(maxXPos.x) < Mathf.Abs(createPos.x))
            maxXPos = createPos;

        if (Mathf.Abs(maxZPos.z) < Mathf.Abs(createPos.z))
            maxZPos = createPos;
    }

    private void CreateStore()
    {
        if (Vector3.Distance(farDistancePos, maxXPos) > Vector3.Distance(farDistancePos, maxXPos))
        {
            //저
            //밥좀?머ㅏㄱ고올겠여
        }
    }
}
