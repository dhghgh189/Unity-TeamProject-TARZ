using System.Collections.Generic;
using UnityEngine;
using Zenject;
public enum MonsterName
{
    Jake, Amber, Size
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] PooledObject _monsterPrefab;

    private List<MonsterFactoryData> monsterFactoryDatas = new();

    [Inject(Id = "Jake")]
    private MonsterFactory jakeFactory;
    [SerializeField] Transform jakePool;

    [Inject(Id = "Amberr")]
    private MonsterFactory amberFactory;
    [SerializeField] Transform amberPool;


    public class MonsterFactoryData
    {
        public Transform PoolTransform;
        public MonsterFactory factory;
    }

    private void Awake()
    {
        monsterFactoryDatas.Add(new MonsterFactoryData() { factory = jakeFactory, PoolTransform = jakePool });
        monsterFactoryDatas.Add(new MonsterFactoryData() { factory = amberFactory, PoolTransform = amberPool });
    }

    public PooledObject CreateMonster(MonsterName monsterName, Vector3 pos)
    {
        MonsterFactoryData temp = monsterFactoryDatas[(int)monsterName];
        foreach (var item in temp.PoolTransform.GetComponentsInChildren<Transform>(true))
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.transform.position = pos;
                item.gameObject.SetActive(true);

                return item.GetComponent<PooledObject>();
            }
        }
        return temp.factory.Create().GetComponent<PooledObject>();
    }

    public void ReturnPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
}