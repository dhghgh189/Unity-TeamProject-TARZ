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

    [Header("Initial Pool Size")]
    [SerializeField] private int initialPoolSize = 4;

    public class MonsterFactoryData
    {
        public Transform PoolTransform;
        public MonsterFactory factory;
    }

    private void Start()
    {
        // 팩토리 데이터를 초기화 및 몬스터 오브젝트 미리 생성
        for (int i = 0; i < (int)MonsterName.Size; i++)
        {
            MonsterFactoryData factoryData = new()
            {
                factory = container.ResolveId<MonsterFactory>(((MonsterName)i).ToString()),
                PoolTransform = poolTransforms[i]
            };
            monsterFactoryDatas.Add(factoryData);

            // 초기 풀 크기만큼 몬스터 생성
            for (int j = 0; j < initialPoolSize; j++)
            {
                PooledObject instance = factoryData.factory.Create();
                instance.gameObject.SetActive(false);
                instance.transform.SetParent(factoryData.PoolTransform);
            }
        }
    }

    public PooledObject CreateMonster(MonsterName monsterName, Transform transform)
    {
        MonsterFactoryData factoryData = monsterFactoryDatas[(int)monsterName];
        foreach (PooledObject pooledObject in factoryData.PoolTransform.GetComponentsInChildren<PooledObject>(true))
        {
            if (!pooledObject.gameObject.activeSelf)
            {
                // 비활성화된 오브젝트 초기화 및 활성화
                MonsterData monsterData = pooledObject.GetComponent<MonsterData>();
                monsterData.CurHp = monsterData.MaxHp;
                monsterData.IsAttacked = false;
                monsterData.IsCatched = false;
                monsterData.IsDead = false;
                pooledObject.CapsuleCollider.enabled = true;
                pooledObject.gameObject.transform.position = transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                Animator monsterAnimator = pooledObject.GetComponent<Animator>();
                monsterAnimator.SetBool("IsDead", false);
                pooledObject.gameObject.SetActive(true);

                return pooledObject;
            }
        }

        // 풀에 사용 가능한 몬스터가 없으면 새로 생성
        PooledObject instance = factoryData.factory.Create();
        instance.GetComponent<NavMeshAgent>().enabled = false;
        instance.gameObject.transform.position = transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        instance.GetComponent<NavMeshAgent>().enabled = true;
        instance.transform.SetParent(factoryData.PoolTransform);

        return instance;
    }

    public void ReturnPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
}