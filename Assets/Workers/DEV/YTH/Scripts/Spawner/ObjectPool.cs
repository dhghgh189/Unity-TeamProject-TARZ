using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] List<PooledObject> pool = new List<PooledObject>();

    [SerializeField] PooledObject _monsterPrefab;

    [SerializeField] int _poolSize;

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            PooledObject instance = Instantiate(_monsterPrefab);
            instance.gameObject.SetActive(false);
            instance.transform.parent = transform;
            instance.ReturnPool = this;
            pool.Add(instance);
        }
    }

    public PooledObject GetPool(Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            PooledObject instance = pool[pool.Count - 1];
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.parent = null;
            instance.gameObject.SetActive(true);
            instance.ReturnPool = this;

            pool.RemoveAt(pool.Count - 1);

            return instance;
        }
        else
        {
            PooledObject instance = Instantiate(_monsterPrefab, position, rotation);
            instance.ReturnPool = this;
            return instance;
        }
    }

    public void ReturnPool(PooledObject instance)
    {
        instance.gameObject.SetActive(false);
        instance.transform.parent = transform;

        pool.Add(instance);
    }
}
