using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MonsterData;

/// <summary>
/// 중력장을 발생하는 스크립트
/// </summary>
public class GravityShoot : MonoBehaviour, ISpec
{
    [SerializeField] SkillSoundScript script;
    [SerializeField] float operationTime;   // 동작하는 시간
    [SerializeField] float force;           // 끌어당기는 힘
    [SerializeField] float range;           // 끌어당기는 범위

    private Coroutine coroutine;
    [SerializeField] Rigidbody rigid;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] float defaultForce;

    private void Awake()
    {
        script = GetComponent<SkillSoundScript>();
    }

    private void Init()
    {
        script.PlaySound();
        GetComponent<SphereCollider>().radius = range;
        Destroy(gameObject, operationTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 끌어당기기
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            MonsterData data = (other.gameObject.GetComponent<MonsterData>() is not null) ? other.gameObject.GetComponent<MonsterData>() : other.gameObject.GetComponentInParent<MonsterData>();
            if (data.MonsterTIer.Equals(MonsterTier.Normal))
            {
                enemies.Add(data.gameObject);
                SetHold(other.gameObject);
                StartCoroutine(StartBoilingRoutine(data.gameObject.transform));
            }
        }
    }
    private IEnumerator StartBoilingRoutine(Transform other)
    {
        /* NavMeshAgent -> Rigidbody 물리(강체) 적용하기 위한 행동 */
        yield return null;
        Debug.Log("초기설정");
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        Debug.Log("물리 적용");
        while (true)
        {
            rigid.AddExplosionForce(defaultForce * force * Time.fixedDeltaTime, transform.position, range);
            // other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, force * Time.deltaTime);
            /* 해당 적용이 다 끝나는 조건 */
            yield return null;
        }
    }

    // 중력장 지속시간이 다 되어서 파괴되는 경우
    private void OnDestroy()
    {
        foreach (var enemy in enemies)
        {
            if (enemy is null) continue;
            SetPut(enemy);
        }
        // 모든 코루틴 종료
        StopAllCoroutines();
    }

    public void SetSpec(Spec spec, int level)
    {
        operationTime = spec.Time(level);
        force = spec.Power(level);
        range = spec.Range(level);

        Init();
    }

    /// <summary>
    /// 잡는 함수 -> 물리(강체)를 사용하기 위한 설정
    /// </summary>
    /// <param name="other">사용 요청을 한 물체</param>
    private void SetHold(GameObject other)
    {
        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        MonsterData monsterData = other.GetComponent<MonsterData>();
        monsterData.IsCatched = true;
        agent.enabled = false;
        rigid.useGravity = true;
        rigid.isKinematic = false;
    }

    /// <summary>
    /// 놓아주는 함수 -> NavMesh를 활성화하기 위한 설정
    /// </summary>
    /// <param name="other"></param>
    private void SetPut(GameObject other)
    {
        /* Rigidbody -> NavMeshAgent 다시 navMesh를 활성화하기 위한 행동 */
        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        MonsterData monsterData = other.GetComponent<MonsterData>();
        agent.enabled = true;
        rigid.useGravity = false;
        rigid.isKinematic = true;
        monsterData.IsCatched = false;
        Debug.Log("원복 끝");
    }
}
