using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private ObjectPool _monsterPool;

    private Transform _spawnPoint;

    private void Awake()
    {
        _monsterPool = GetComponent<ObjectPool>();
        _spawnPoint = transform.Find("MonsterSpawnPoint");
    }

    public void Spawn()
    {
        PooledObject instance = _monsterPool.GetPool(_spawnPoint.position, _spawnPoint.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("몬스터 소환!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Spawn();
            Destroy(gameObject);
        }
    }
}
