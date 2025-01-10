using System.Collections;
using UnityEngine;
using Zenject;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject startRoomPrefab;
    [SerializeField] GameObject storePrefab;
    [SerializeField] GameObject wallDestroyer;
    [SerializeField] RoomChecker roomChecker;
    [SerializeField] GameObject[] monsterSpawners;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject[] traps;
    [SerializeField] GameObject trapRoomPrefab;

    [SerializeField] GameObject movePotalPrefab;
    [SerializeField] GameObject scenePotalPrefab;
    [SerializeField] GameObject bossRoomPrefab;
    [SerializeField] GameObject NPCPrefab;
    [SerializeField] GameObject[] SpecialPrefab;

    private Transform bossRoomTransform;

    // 랜덤한 방향을 담을 배열
    private Vector3[] createDir = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    // 파괴자의 y좌표
    private Vector3 destroyerY = new Vector3(0, 7f, 0);

    // 가장 먼 방을 찾기 위한 변수
    private Vector3 farDistancePos;
    private Vector3 bossRoomDir;
    private Vector3 maxZPos;
    private Vector3 maxXPos;

    // 방을 생성할 위치
    private Vector3 createPos;

    private int random;
    private int roomCount;

    [SerializeField] int maxTrapCount;

    [Inject] PlayerController playerController;
    [Inject] InGameSaveData saveData;

    [SerializeField] ChapterManager chapterManager;

    private void Start()
    {
        ChapterSaveData chapterSaveData = saveData.chapterSaveData;
        StageInfo stageInfo = chapterManager.stageInfos[chapterSaveData.StageNum];
        roomCount = stageInfo.RoomCount;

        // 시작 지점 생성 (비전투 비행기 방)
        Instantiate(startRoomPrefab, createPos, Quaternion.identity, transform);

        // 일정확률로 돌발퀘스트 NPC 생성
        if (Util.IsRandom(50))
        {
            Instantiate(NPCPrefab, createPos + Vector3.forward * 10f, Quaternion.identity, transform);
        }

        // 절차적 맵 생성 시작
        CreateBossRoom();
        StartCoroutine(MapCreater());
    }

    private void CreateBossRoom()
    {
        bossRoomTransform = Instantiate(bossRoomPrefab, new Vector3(3000f, 0, 3000f), Quaternion.identity).transform;
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
                random = Random.Range(0, 4);
                while (createPos.z == 50 && random == 1)
                {
                    random = Random.Range(0, 4);
                }
                createPos += createDir[random] * 50f;
                wallDestroyer.transform.position = destroyerY + createPos - createDir[random] * 25f;
                continue;
            }


            if (maxTrapCount > 0 && Util.IsRandom(50))
            {
                maxTrapCount--;
                Debug.Log("트랩 방 생성!");
                Transform roomTransform = Instantiate(trapRoomPrefab, createPos, Quaternion.identity, transform).transform;
                // 트랩만 있는 방 생성
            }
            else
            {
                // 방 생성
                Transform roomTransform = Instantiate(roomPrefab, createPos, Quaternion.identity, transform).transform;
                Instantiate(obstacles[Random.Range(0, obstacles.Length)], createPos, Quaternion.identity, transform);
                Instantiate(monsterSpawners[Random.Range(0, monsterSpawners.Length)], createPos, Quaternion.identity, roomTransform);
                Instantiate(SpecialPrefab[Random.Range(0, SpecialPrefab.Length)], createPos + new Vector3(Random.Range(-15, 15), 3f, Random.Range(-15, 15)), Quaternion.identity, roomTransform);
            }
            

            // 상점, 보스방 생성을 위한 가장 먼 방 체크
            FindFarRoomPos();

            // 랜덤 방향 지정
            random = Random.Range(0, 4);
            while (createPos.z == 50 && random == 1)
            {
                random = Random.Range(0, 4);
            }
            if (i == 0)
            {
                random = 0;
            }
            createPos += createDir[random] * 50f;

            // 진행 방향의 벽 제거
            if (i == roomCount - 1)
                break;
            yield return Util.GetDelay(0.05f);
            wallDestroyer.transform.position = destroyerY + createPos - createDir[random] * 25f;
        }

        // 보스방 방향 지정
        SetBossRoomDir();

        yield return Util.GetDelay(0.05f);

        // 상점 생성
        CreateStore();

        yield return Util.GetDelay(0.05f);

        // 보스룸 통로 생성
        for (int i = 0; i < 2; i++)
        {
            Instantiate(roomPrefab, farDistancePos, Quaternion.identity, transform);
            yield return Util.GetDelay(0.05f);

            if (i == 1)
            {
                Instantiate(movePotalPrefab, farDistancePos + Vector3.up, Quaternion.identity).GetComponent<MovePotal>().SetTarget(bossRoomTransform.position);
            }

            wallDestroyer.transform.position = destroyerY + farDistancePos - bossRoomDir * 25f;
            farDistancePos += bossRoomDir * 50;
        }

        yield return Util.GetDelay(0.05f);
        Destroy(wallDestroyer.gameObject);
        Destroy(roomChecker.gameObject);

        playerController.gameObject.SetActive(true);
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
        float setStoreDir = 0;
        if (Vector3.Distance(farDistancePos, maxXPos) > Vector3.Distance(farDistancePos, maxZPos))
        {
            setStoreDir = maxXPos.x < 0 ? -1 : 1;
            maxXPos += Vector3.right * 50f * setStoreDir;
            Instantiate(storePrefab, maxXPos, Quaternion.identity);
            wallDestroyer.transform.position = destroyerY + maxXPos - Vector3.right * 25f * setStoreDir;
        }
        else
        {
            setStoreDir = maxZPos.z < 0 ? -1 : 1;
            maxZPos += Vector3.forward * 50f * setStoreDir;
            Instantiate(storePrefab, maxZPos, Quaternion.identity);
            wallDestroyer.transform.position = destroyerY + maxZPos - Vector3.forward * 25f * setStoreDir;
        }
    }
}
