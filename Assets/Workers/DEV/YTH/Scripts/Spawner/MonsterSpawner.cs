using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] List<MonsterSpwanInfo> monsterSpwanInfos;

    private ObjectPool _monsterPool;
    private Transform[] _spawnPoints;

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().Skip(1).ToArray();
    }

    private void Start()
    {
        _monsterPool = FindAnyObjectByType<ObjectPool>();
    }

    public void Spawn()
    {
        RoomBehaviour roomBehaviour = transform.parent.GetComponent<RoomBehaviour>();
        int temp = 0;
        for (int i = 0; i < monsterSpwanInfos.Count; i++)
        {
            for (int j = 0; j < monsterSpwanInfos[i].MonsterCount; j++)
            {
                PooledObject pooledObject = _monsterPool.CreateMonster(monsterSpwanInfos[i].monsterName, _spawnPoints[temp++ % _spawnPoints.Length]);

                pooledObject.OnDie += roomBehaviour.MonsterCountChange;
            }
        }
        roomBehaviour.CloseWall();
        roomBehaviour.MonsterCount = temp;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("몬스터 소환!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Spawn();
        }
    }

    [Serializable]
    public class MonsterSpwanInfo
    {
        public MonsterName monsterName;
        public int MonsterCount;
    }
}
