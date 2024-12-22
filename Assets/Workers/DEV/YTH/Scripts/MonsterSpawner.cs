using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] ObjectPool _monsterPool;

    [SerializeField] Transform _spawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        PooledObject instance = _monsterPool.GetPool(_spawnPoint.position, _spawnPoint.rotation);
    }
}
