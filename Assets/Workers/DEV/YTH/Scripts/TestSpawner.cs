using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] GameObject _jake;

    private ObjectPool _pool;
    private void Start()
    {
        _pool = FindAnyObjectByType<ObjectPool>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _pool.CreateMonster(MonsterName.Jake, transform);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            _pool.CreateMonster(MonsterName.Range, transform);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            _pool.CreateMonster(MonsterName.FrogZombie, transform);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            _pool.CreateMonster(MonsterName.ReviveZombie, transform);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            _pool.CreateMonster(MonsterName.DungeonEliteB, transform);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            _pool.CreateMonster(MonsterName.DungeonEliteA, transform);
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            _pool.CreateMonster(MonsterName.EliteA, transform);
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            _pool.CreateMonster(MonsterName.EliteB, transform);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            _pool.CreateMonster(MonsterName.Bomber, transform);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            _pool.CreateMonster(MonsterName.JackTheRipper, transform);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            _pool.CreateMonster(MonsterName.Arnold, transform);
        }
    }
}
