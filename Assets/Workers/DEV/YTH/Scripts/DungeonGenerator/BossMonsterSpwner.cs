using UnityEngine;

public class BossMonsterSpwner : MonoBehaviour
{
    private SaveData saveData;
    private ChapterManager chapterManager;
    private ObjectPool _monsterPool;
    private void Start()
    {
        _monsterPool = FindAnyObjectByType<ObjectPool>();
        chapterManager = FindAnyObjectByType<ChapterManager>();
        saveData = chapterManager.saveData;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("보스 소환!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Spawn();
        }
    }
    public void Spawn()
    {
        BossRoomBehaviour bossRoomBehaviour = transform.parent.GetComponent<BossRoomBehaviour>();
        foreach (MonsterName monsterName in chapterManager.stageInfos[saveData.chapterSaveData.StageNum].BossName)
        {
            PooledObject pooledObject = _monsterPool.CreateMonster(monsterName, transform);
            pooledObject.OnDie += bossRoomBehaviour.BossCountChange;
            bossRoomBehaviour.BossCount++;
        }
        Destroy(gameObject);
    }
}
