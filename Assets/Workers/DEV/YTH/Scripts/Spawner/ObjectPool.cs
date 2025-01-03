using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
public enum MonsterName
{
    Jake, Amber, Arnold, Bomber, FrogZombie, JackTheRipper, Range, ReviveZombie, EliteA, EliteB, DungeonEliteA, DungeonEliteB, Size
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
        MonsterFactoryData factoryData = monsterFactoryDatas[(int)monsterName];
        foreach (PooledObject pooledObject in factoryData.PoolTransform.GetComponentsInChildren<PooledObject>(true))
        {
            if (!pooledObject.gameObject.activeSelf)
            {
                MonsterData monsterData = pooledObject.GetComponent<MonsterData>();
                monsterData.CurHp = monsterData.MaxHp;
                monsterData.IsAttacked = false; 
                monsterData.IsCatched = false; 
                pooledObject.gameObject.transform.position = transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                pooledObject.gameObject.SetActive(true);
                

                return pooledObject;
            }
        }
        PooledObject instance = factoryData.factory.Create();
        instance.GetComponent<NavMeshAgent>().enabled = false;
        instance.gameObject.transform.position = transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)); ;
        instance.GetComponent<NavMeshAgent>().enabled = true;

        return instance;
    }

    public void ReturnPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
}