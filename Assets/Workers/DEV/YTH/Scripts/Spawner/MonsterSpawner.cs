using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] List<MonsterSpwanInfo> monsterSpwanInfos;

    private ObjectPool _monsterPool;
    private Transform[] _spawnPoints;
    RoomBehaviour roomBehaviour;

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().Skip(1).ToArray();
        roomBehaviour = transform.parent.GetComponent<RoomBehaviour>();
    }

    private void Start()
    {
        _monsterPool = FindAnyObjectByType<ObjectPool>();
    }

    public void Spawn()
    {
        int temp = 0;
        for (int i = 0; i < monsterSpwanInfos.Count; i++)
        {
            for (int j = 0; j < monsterSpwanInfos[i].MonsterCount; j++)
            {
                PooledObject pooledObject = _monsterPool.CreateMonster(monsterSpwanInfos[i].monsterName, _spawnPoints[temp++ % _spawnPoints.Length]);

                pooledObject.OnDie += roomBehaviour.MonsterCountChange;
            }
        }
        roomBehaviour.MonsterCount = temp;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("몬스터 소환!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            StartCoroutine(SpawnCoroutine());
        }
    }

    IEnumerator SpawnCoroutine()
    {
        roomBehaviour.CloseWall();
        yield return Util.GetDelay(3f);
        foreach (var item in _spawnPoints)
        {
            EffectManager.instance.ParticlePlay("GasExplosionPink", 5f, item.position, Quaternion.identity);
        }
        Spawn();
    }

    [Serializable]
    public class MonsterSpwanInfo
    {
        public MonsterName monsterName;
        public int MonsterCount;
    }
}
