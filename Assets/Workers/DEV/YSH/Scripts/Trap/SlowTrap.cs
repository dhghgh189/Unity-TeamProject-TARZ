using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public PlayerController TargetPlayer { get { return targetPlayer; } }

    private Dictionary<MonsterData, float> monsterBaseSpeedDic = new Dictionary<MonsterData, float>();
    public Dictionary<MonsterData, float> MonsterBaseSpeedDic { get { return monsterBaseSpeedDic; } }

    private SlowTrapCore core;

    private List<StatusEffect> effects = new List<StatusEffect>();
    public List<StatusEffect> Effects { get { return effects; } }

    // 다른 slow trap과 범위가 겹친경우 저장
    private List<SlowTrap> overlapTraps = new List<SlowTrap>();
    // 동일한 effect가 다른 slow trap에 있는 경우 저장
    private List<Collider> hitColliders = new List<Collider>();
    public List<Collider> HitColliders { get { return hitColliders; } }

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

        foreach (StatusEffect effect in effects)
        {
            if (effect == null) continue; 
            
            effect.Rollback(slowPercent, out _);
        }
    }

    protected override void Init()
    {
        // 트랩 범위를 보여주기 위한 Indicator 크기 설정
        coll = GetComponent<CapsuleCollider>();
        coll.radius = radius;
        indicatorTrf.sizeDelta = new Vector2(radius * 2.1f, radius * 2.1f);

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
        // enter된 물체가 slow trap이면
        if (other.gameObject.CompareTag(gameObject.tag))
        {
            SlowTrap slowTrap = other.GetComponent<SlowTrap>();
            if (slowTrap == null || overlapTraps.Contains(slowTrap))
            {
                Debug.Log("<color=red>slowTrap exception</color>");
            }
            overlapTraps.Add(slowTrap);
            Debug.Log($"SlowTrap : {gameObject.name}과 {other.name} 겹침!");
            return;
        }

        if (!isActive)
            return;

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")
            && other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        if (hitColliders.Contains(other))
        {
            Debug.Log("<color=red>SlowTrap : Exception!</color>");
            return;
        }

        // 들어온 물체를 저장
        hitColliders.Add(other);

        // test
        Debug.Log($"SlowTrap : {other} entered {gameObject.name}");

        StatusEffect effect = other.GetComponentInChildren<StatusEffect>();
        if (effect != null)
        {
            // 겹쳐있는 트랩들 중 이미 해당 객체를 저장해둔 트랩이 있는지 찾는다.
            SlowTrap overlap = GetOverlapTrap(other);

            // 해당되는 트랩이 있으면 아무것도 안한다.
            if (overlap != null)
            {
                Debug.Log($"SlowTrap : {overlap.name}에서 이미 동일한 effect를 실행했다!");
            }
            else
            {
                Debug.Log($"SlowTrap : {gameObject.name}에 effect 추가");
                effects.Add(effect);
                effect.Slow(slowPercent, out var target);
            }
        }
        else
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Stat.SpeedRate = (1f - slowPercent);
                targetPlayer = player;
                return;
            }
        }
    }

    public SlowTrap GetOverlapTrap(Collider collider)
    {
        // LINQ를 통해 overlapTraps 중 collider를 저장하고 있는 트랩을 찾는다
        var query = from overlaps in overlapTraps
                    where overlaps.HitColliders.Contains(collider)
                    select overlaps;

        if (query.Count() <= 0)
            return null;

        return query.First();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive)
            return;

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")
            && other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        // test
        Debug.Log($"SlowTrap : {other} exited {gameObject.name}");

        // 충돌한 물체 리스트에서 제거
        hitColliders.Remove(other);

        StatusEffect effect = other.GetComponentInChildren<StatusEffect>();
        if (effect != null)
        {
            // exit하는 객체의 effect가 자신의 effects에 없으면 아무것도 안한다.
            if (!effects.Contains(effect))
            {
                Debug.Log($"SlowTrap : {gameObject.name}의 effects가 아니므로 아무것도 하지않는다.");
                return;
            }

            // 겹쳐있는 트랩들 중 exit되는 물체를 저장해둔 트랩이 있는지 찾는다.
            SlowTrap overlap = GetOverlapTrap(other);

            if (overlap != null)
            {
                Debug.Log($"SlowTrap : {gameObject.name}의 Effect를 {overlap.name}에게 전달!");
                // 겹쳐있는 트랩에 자신의 effect를 넘긴다
                overlap.Effects.Add(effect);
                // 자신의 effects에서는 effect를 제거한다.
                effects.Remove(effect);
            }
            else
            {
                Debug.Log($"SlowTrap : {gameObject.name}에서 effect 삭제");
                effect.Rollback(slowPercent, out var target);
                effects.Remove(effect);
            }
        }
        else
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // 겹쳐있는 트랩들 중 exit되는 물체를 저장해둔 트랩이 있는지 찾는다.
                SlowTrap overlap = GetOverlapTrap(other);

                // 동일한 collider를 저장하는 트랩이 있다면 rate를 원복하지 않는다.
                if (overlap != null)
                    return;

                player.Stat.SpeedRate = 1f;
                targetPlayer = null;
                return;
            }
        }
    }

    private void OnDestroy()
    {
        Deactivate();
        monsterBaseSpeedDic.Clear();
    }
}
