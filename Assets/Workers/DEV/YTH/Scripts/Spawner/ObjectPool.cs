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

    [Inject] DiContainer container;

    [Header("Pool Transforms")]
    [SerializeField] Transform[] poolTransforms;


    public class MonsterFactoryData
    {
        public Transform PoolTransform;
        public MonsterFactory factory;
    }

    private void Start()
    {
        // 컨데이너를 바인딩 해서 ResolveId로 팩토리들을 가져오고 리스트에 추가
        for (int i = 0; i < (int)MonsterName.Size; i++)
        {
            monsterFactoryDatas.Add(new MonsterFactoryData() { factory = container.ResolveId<MonsterFactory>(((MonsterName)i).ToString()), PoolTransform = poolTransforms[i] });
        }
    }

    public PooledObject CreateMonster(MonsterName monsterName, Transform transform)
    {
        MonsterFactoryData temp = monsterFactoryDatas[(int)monsterName];
        foreach (var item in temp.PoolTransform.GetComponentsInChildren<PooledObject>(true))
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