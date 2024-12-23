using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private MonsterSpawner _monsterSpawner;

    private void Start()
    {
        _monsterSpawner = GetComponentInParent<MonsterSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _monsterSpawner.Spawn();
        }
    }
}
