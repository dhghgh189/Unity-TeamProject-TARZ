using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class MapGenerator : MonoBehaviour
{
    [Header("맵 생성 프리팹")]
    [SerializeField] GameObject wallDestroyer;
    [SerializeField] RoomChecker roomChecker;
    [Header("")]
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject startRoomPrefab;
    [SerializeField] GameObject storePrefab;
    [SerializeField] GameObject bossRoomPrefab;
    [SerializeField] GameObject[] trapRoomPrefab;
    [Header("")]
    [SerializeField] GameObject[] monsterSpawners; // 일반몹 스포너 프리팹 
    [SerializeField] GameObject[] eliteSpawners;   // 엘리트 몬스터 스포너 프리팹 
    [SerializeField] GameObject[] obstacles;       // 장애물 프리팹
    [SerializeField] GameObject[] SpecialPrefab;   // 특수 오브젝트 프리팹
    [Header("")]
    [SerializeField] GameObject NPCPrefab;         // 돌발 퀘스트 NPC 프리팹
    [SerializeField] GameObject movePotalPrefab;
    [SerializeField] GameObject scenePotalPrefab;

    [Header("세팅")]
    [SerializeField] int maxTrapCount;
    [SerializeField] float trapRoom_P;
    [SerializeField] int[] maxEliteRoomCount = { 1, 3, 5};
    [SerializeField] float[] eliteRoom_P = { 20, 50, 60 };

    [Header("비전투 구역 프리팹")]
    [SerializeField] GameObject bigRobot;
    [SerializeField] GameObject robot;
    [SerializeField] GameObject coupang;

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

    private ChapterManager chapterManager;

    private int random;
    private int roomCount;
    private int noBattle = 0;
    private int stageNum;

    private int[] noBattleIndex; // 비전투구역 생성 갯수 

    [Inject] PlayerController playerController;
    [Inject] InGameSaveData saveData;

    private void Awake()
    {
        chapterManager = GetComponentInChildren<ChapterManager>();
    }

    private void Start()
    {
        ChapterSaveData chapterSaveData = saveData.chapterSaveData;
        StageInfo stageInfo = chapterManager.stageInfos[chapterSaveData.StageNum];
        roomCount = stageInfo.RoomCount;
        stageNum = chapterSaveData.StageNum;

        noBattleIndex = new int[chapterSaveData.StageNum + 1];
        for (int i = 0; i < noBattleIndex.Length; i++)
        {
            int random = UnityEngine.Random.Range(1, roomCount);
            if (noBattleIndex.Contains(random))
            {
                i--;
                continue;
            }
            noBattleIndex[i] = random;

        }

        // 시작 지점 생성 (비전투 비행기 방)
        Instantiate(startRoomPrefab, createPos, Quaternion.identity, transform);

        // 일정확률로 돌발퀘스트 NPC 생성
        // TODO : 추후 폴리싱에 구현 예정
        /*if (Util.IsRandom(50))
        {
            Instantiate(NPCPrefab, createPos + Vector3.forward * 10f, Quaternion.identity, transform);
        }*/

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
                random = UnityEngine.Random.Range(0, 4);
                while (createPos.z == 50 && random == 1)
                {
                    random = UnityEngine.Random.Range(0, 4);
                }
                createPos += createDir[random] * 50f;
                wallDestroyer.transform.position = destroyerY + createPos - createDir[random] * 25f;
                continue;
            }

            // 비전투 구역 생성
            if (noBattle < noBattleIndex.Length && i == noBattleIndex[noBattle])
            {
                GameObject gameObj = bigRobot;

                // 스테이지에 따른 비전투 구역 종류 확률
                switch (stageNum)
                {
                    case 0:
                        gameObj = (UnityEngine.Random.Range(0, 1f)) switch
                        {
                            < 0 => bigRobot,                    // 대형로봇
                            > 0 and < 0.1f => robot,         // 로봇
                            _ => coupang,                      // 택배 
                        };
                        break;

                    case 1:
                        gameObj = (UnityEngine.Random.Range(0, 1f)) switch
                        {
                            < 0.03f => bigRobot,                    // 대형로봇
                            > 0.03f and < 0.2f => robot,         // 로봇
                            _ => coupang,                          // 택배 
                        };
                        break;

                    case 2:
                        gameObj = (UnityEngine.Random.Range(0, 1f)) switch
                        {
                            < 0.05f => bigRobot,                    // 대형로봇
                            > 0.05f and < 0.3f => robot,         // 로봇
                            _ => coupang,                          // 택배 
                        };
                        break;
                }

                Transform roomTransform = Instantiate(gameObj, createPos, Quaternion.identity, transform).transform;
            }
            // 함정 룸 생성
            else if (maxTrapCount > 0 && Util.IsRandom(trapRoom_P))
            {
                maxTrapCount--;
                Debug.Log("트랩 방 생성!");
                Transform roomTransform = Instantiate(trapRoomPrefab[UnityEngine.Random.Range(0, trapRoomPrefab.Length)], createPos, Quaternion.identity, transform).transform;
                // 트랩만 있는 방 생성
            }
            // 엘리트 몬스터 룸 생성
            else if (maxEliteRoomCount[saveData.chapterSaveData.StageNum] > 0 && Util.IsRandom(eliteRoom_P[saveData.chapterSaveData.StageNum]))
            {
                maxEliteRoomCount[saveData.chapterSaveData.StageNum]--;
                Transform roomTransform = Instantiate(roomPrefab, createPos, Quaternion.identity, transform).transform;
                Instantiate(obstacles[UnityEngine.Random.Range(0, obstacles.Length)], createPos, Quaternion.identity, transform);
                Instantiate(eliteSpawners[UnityEngine.Random.Range(0, eliteSpawners.Length)], createPos, Quaternion.identity, roomTransform);
                Instantiate(SpecialPrefab[UnityEngine.Random.Range(0, SpecialPrefab.Length)], createPos + new Vector3(UnityEngine.Random.Range(-15, 15), 3f, UnityEngine.Random.Range(-15, 15)), Quaternion.identity, roomTransform);
            }
            // 일반 몬스터 룸 생성
            else
            {
                Transform roomTransform = Instantiate(roomPrefab, createPos, Quaternion.identity, transform).transform;
                Instantiate(obstacles[UnityEngine.Random.Range(0, obstacles.Length)], createPos, Quaternion.identity, transform);
                Instantiate(monsterSpawners[UnityEngine.Random.Range(0, monsterSpawners.Length)], createPos, Quaternion.identity, roomTransform);
                Instantiate(SpecialPrefab[UnityEngine.Random.Range(0, SpecialPrefab.Length)], createPos + new Vector3(UnityEngine.Random.Range(-15, 15), 3f, UnityEngine.Random.Range(-15, 15)), Quaternion.identity, roomTransform);
            }

            // 상점, 보스방 생성을 위한 가장 먼 방 체크
            FindFarRoomPos();

            // 랜덤 방향 지정
            random = UnityEngine.Random.Range(0, 4);
            while (createPos.z == 50 && random == 1)
            {
                random = UnityEngine.Random.Range(0, 4);
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

        // 사운드 재생 (임시)
        if (SoundManager.BGM.clip != SoundManager.SoundData_UI.FieldBGM)
            SoundManager.PlayBGM(SoundManager.SoundData_UI.FieldBGM);
    }

    private void SetBossRoomDir()
    {
        while (true)
        {
            random = UnityEngine.Random.Range(0, 4);
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
            Instantiate(storePrefab, maxXPos, Quaternion.Euler(Vector3.up * 90 * -setStoreDir));
            wallDestroyer.transform.position = destroyerY + maxXPos - Vector3.right * 25f * setStoreDir;
        }
        else
        {
            setStoreDir = maxZPos.z < 0 ? -1 : 1;
            maxZPos += Vector3.forward * 50f * setStoreDir;
            Instantiate(storePrefab, maxZPos, Quaternion.Euler(Vector3.up * 180 * (setStoreDir == 1 ? 1 : 0)));
            wallDestroyer.transform.position = destroyerY + maxZPos - Vector3.forward * 25f * setStoreDir;
        }
    }
}
