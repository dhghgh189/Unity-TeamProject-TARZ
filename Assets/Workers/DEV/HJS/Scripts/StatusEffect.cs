using System.Collections;
using System.Linq;
using UnityEngine;

public enum StatusEffectType { Frost, Dust, Poison }
public enum Target { None, Player, Monster }
public class StatusEffect : MonoBehaviour
{
    public PlayerController player;
    public MonsterData monster;

    private Coroutine slowRoutine;

    [Header("Status Effect Type")]
    [SerializeField] StatusEffectStruct[] effects;

    public void StartEffect(StatusEffectType type) => effects.Where(x => x.type.Equals(type)).First().effect.Play();
    public void StopEffect(StatusEffectType type) => effects.Where(x => x.type.Equals(type)).First().effect.Stop();

    private void Start()
    {
        // 태그가 플레이어다 -> 플레이어에 속해있다
        if (transform.parent.CompareTag("Player")) player = GetComponentInParent<PlayerController>();
        // 태그가 플레이어가 아니다 -> 몬스터에 속해있다
        else monster = GetComponentInParent<MonsterData>();
    }
    public void Slow(float amount, out Target target)
    {
        if (player != null)
        {
            player.Stat.SpeedRate = (1f - amount);
            target = Target.Player;
        }
        else if (monster != null)
        {
            monster.agent.speed *= (1f - amount);
            target = Target.Monster;
        }
        else
        {
            target = Target.None;
        }
        Debug.Log($"{gameObject.name} is Slow!");
        StartEffect(StatusEffectType.Frost);
    }

    public void Rollback(float amount, out Target target)
    {
        if (player != null)
        {
            player.Stat.SpeedRate = 1f;
            target = Target.Player;
        }
        else if (monster != null)
        {
            monster.agent.speed /= (1f - amount);
            target = Target.Monster;
        }
        else
        {
            target = Target.None;
        }
        StopEffect(StatusEffectType.Frost);
    }

    public void SlowSkill(float amount, float time) => StartCoroutine(SlowRoutine(amount, time));

    private IEnumerator SlowRoutine(float amount, float time)
    {
        Slow(amount, out _);
        yield return Util.GetDelay(time);
        Rollback(amount, out _);
    }
}

[System.Serializable]
public struct StatusEffectStruct
{
    public StatusEffectType type;
    public ParticleSystem effect;
}
