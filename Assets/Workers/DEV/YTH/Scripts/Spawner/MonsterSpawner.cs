using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] ObjectPool _monsterPool;

    [SerializeField] Collider[] _spawnTriggers;

    [SerializeField] Transform _spawnPoint;

    private void Start()
    {
        foreach (Collider spawnTrigger in _spawnTriggers)
        {
           spawnTrigger.gameObject.AddComponent<SpawnTrigger>();
        }
    }

    public void Spawn()
    {
        PooledObject instance = _monsterPool.GetPool(_spawnPoint.position, _spawnPoint.rotation);
    }

   
}
