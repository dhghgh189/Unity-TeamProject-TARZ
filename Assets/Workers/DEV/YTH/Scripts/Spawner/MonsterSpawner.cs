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

    private MapGenerator _generator;
    private UNQuest_Interaction _quest;

    Coroutine coroutine;

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().Skip(1).ToArray();
        roomBehaviour = transform.parent.GetComponent<RoomBehaviour>();
    }

    private void Start()
    {
        _generator = FindAnyObjectByType<MapGenerator>();
        _monsterPool = FindAnyObjectByType<ObjectPool>();
    }

    public void Spawn()
    {
        // 참조가 없는 경우 한번만 찾는다
        if (_generator.IsNpcExist && _quest == null)
        {
            _quest = FindAnyObjectByType<UNQuest_Interaction>();
        }

        int temp = 0;
        for (int i = 0; i < monsterSpwanInfos.Count; i++)
        {
            for (int j = 0; j < monsterSpwanInfos[i].MonsterCount; j++)
            {
                PooledObject pooledObject = _monsterPool.CreateMonster(monsterSpwanInfos[i].monsterName, _spawnPoints[temp++ % _spawnPoints.Length]);

                pooledObject.OnDie += roomBehaviour.MonsterCountChange;

                // quest가 진행중이라면
                if (_quest && _generator.IsNpcExist && _quest.IsOngoing)
                {
                    pooledObject.OnDie += _quest.OnChangeCount;
                }
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

            if (coroutine == null)
                coroutine = StartCoroutine(SpawnCoroutine());
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
