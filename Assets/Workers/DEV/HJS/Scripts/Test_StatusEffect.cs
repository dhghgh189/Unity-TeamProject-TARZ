using System.Collections;
using System.Linq;
using UnityEngine;

public enum Test_StatusEffectType { Frost, Dust, Poison }
public enum Test_Target { None, Player, Monster }
public class Test_StatusEffect : MonoBehaviour
{
    // Slow Queue로 먼저 들어온 친구들 부터 정보 처리
    public PlayerController player;
    public MonsterData monster;
    private float backup = -1f;

    private Coroutine slowRoutine;

    [Header("Status Effect Type")]
    [SerializeField] StatusEffectStruct[] effects;

    public void StartEffect(Test_StatusEffectType type) => effects.Where(x => x.type.Equals(type)).First().effect.Play();
    public void StopEffect(Test_StatusEffectType type) => effects.Where(x => x.type.Equals(type)).First().effect.Stop();

    private void Start()
    {
        // 태그가 플레이어다 -> 플레이어에 속해있다
        if (transform.parent.CompareTag("Player")) player = GetComponentInParent<PlayerController>();
        // 태그가 플레이어가 아니다 -> 몬스터에 속해있다
        else monster = GetComponentInParent<MonsterData>();
    }
    public void Slow(float amount, out Test_Target target)
    {
        if (player != null)
        {
            player.Stat.SpeedRate = (1f - amount);
            target = Test_Target.Player;
        }
        else if (monster != null)
        {
            backup = monster.agent.speed;
            monster.agent.speed *= (1f - amount);
            target = Test_Target.Monster;
        }
        else
        {
            target = Test_Target.None;
        }
    }

    public void Rollback(out Test_Target target)
    {
        if (player != null)
        {
            player.Stat.SpeedRate = 1f;
            target = Test_Target.Player;
        }
        else if (monster != null)
        {
            monster.agent.speed = backup;
            backup = -1;
            target = Test_Target.Monster;
        }
        else
        {
            target = Test_Target.None;
        }
    }
}

[System.Serializable]
public struct StatusEffectStruct
{
    public Test_StatusEffectType type;
    public ParticleSystem effect;
}
