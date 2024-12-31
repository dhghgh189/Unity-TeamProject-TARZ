using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
public enum MonsterName
{
    Jake, Amber, Size
}

public class ObjectPool : MonoBehaviour
{
    private List<MonsterFactoryData> monsterFactoryDatas = new();

    [Inject(Id = "Jake")]
    private MonsterFactory jakeFactory;
    [SerializeField] Transform jakePool;

    [Inject(Id = "Amber")]
    private MonsterFactory amberFactory;
    [SerializeField] Transform amberPool;


    public class MonsterFactoryData
    {
        public Transform PoolTransform;
        public MonsterFactory factory;
    }

    private void Start()
    {
        monsterFactoryDatas.Add(new MonsterFactoryData() { factory = jakeFactory, PoolTransform = jakePool });
        monsterFactoryDatas.Add(new MonsterFactoryData() { factory = amberFactory, PoolTransform = amberPool });
    }

    public PooledObject CreateMonster(MonsterName monsterName, Transform transform)
    {
        MonsterFactoryData temp = monsterFactoryDatas[(int)monsterName];
        foreach (var item in temp.PoolTransform.GetComponentsInChildren<Transform>(true))
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.transform.position = transform.position;
                item.gameObject.SetActive(true);

                return item.GetComponent<PooledObject>();
            }
        }
        PooledObject instance = temp.factory.Create();
        instance.transform.position = transform.position;

        return instance;


    }

    public void ReturnPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
}