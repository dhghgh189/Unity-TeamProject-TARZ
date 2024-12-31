using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] ObjectPool _monsterPool;

    [SerializeField] Transform _spawnPoint;

    private void Awake()
    {
        /*_spawnPoint = transform.Find("MonsterSpawnPoint");*/
    }

 
    public void Spawn()
    {
        PooledObject jake = _monsterPool.CreateMonster(MonsterName.Jake, _spawnPoint);
        PooledObject amber = _monsterPool.CreateMonster(MonsterName.Amber, _spawnPoint);

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
