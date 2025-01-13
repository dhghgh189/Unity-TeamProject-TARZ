using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : Trap
{
    [SerializeField] private float radius;
    [SerializeField] private int hitToDestroy;

    [Range(0, 1)]
    [SerializeField] private float slowPercent;

    [SerializeField] private float rotateSpeed;

    private CapsuleCollider coll;

    [SerializeField] private RectTransform indicatorTrf;

    private PlayerController targetPlayer;
    private Dictionary<MonsterData, float> monsterBaseSpeedDic = new Dictionary<MonsterData, float>();

    private SlowTrapCore core;

    public override void Activate()
    {
        isActive = true;
        indicatorTrf.gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        isActive = false;
        indicatorTrf.gameObject.SetActive(false);

        if (targetPlayer != null)
        {
            targetPlayer.Stat.SpeedRate = 1f;
            targetPlayer = null;
        }

        foreach (MonsterData monster in monsterBaseSpeedDic.Keys)
        {
            if (!monster.IsDead && monster != null)
                monster.agent.speed = monsterBaseSpeedDic[monster];
        }
    }

    protected override void Init()
    {
        // 트랩 범위를 보여주기 위한 Indicator 크기 설정
        coll = GetComponent<CapsuleCollider>();
        coll.radius = radius;
        indicatorTrf.sizeDelta = new Vector2(radius * 2f, radius * 2f);

        // Core 체력 설정
        core = GetComponentInChildren<SlowTrapCore>();
        core.SetCoreHealth(hitToDestroy);

        base.Init();
    }

    private void Update()
    {
        if (!isActive)
            return;

        // 회전시켜 동작되는 듯한 효과 연출
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
            return;    

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")
            && other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Stat.SpeedRate = (1f - slowPercent);
            targetPlayer = player;
            return;
        }

        MonsterData monster = other.GetComponent<MonsterData>();
        if (monster != null)
        {
            if (!monsterBaseSpeedDic.TryAdd(monster, monster.agent.speed))
            {
                monsterBaseSpeedDic.Add(monster, monster.agent.speed);
            }
            monster.agent.speed *= (1f - slowPercent);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive)
            return;

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")
            && other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Stat.SpeedRate = 1f;
            targetPlayer = null;
            return;
        }

        MonsterData monster = other.GetComponent<MonsterData>();
        if (monster != null)
        {
            monster.agent.speed = monsterBaseSpeedDic[monster];
            monsterBaseSpeedDic[monster] = 0;
            return;
        }
    }

    private void OnDestroy()
    {
        Deactivate();
        monsterBaseSpeedDic.Clear();
    }
}
