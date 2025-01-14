using System.Collections;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;

public enum Test_StatusEffectType { Frost, Dust, Poison }
public enum Test_Target { None, Player, Monster }
public class Test_StatusEffect : MonoBehaviour
{
    public PlayerController player;
    public MonsterData monster;

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
            monster.agent.speed *= (1f - amount);
            target = Test_Target.Monster;
        }
        else
        {
            target = Test_Target.None;
        }
        Debug.Log($"{gameObject.name} is Slow!");
    }

    public void Rollback(float amount, out Test_Target target)
    {
        if (player != null)
        {
            player.Stat.SpeedRate = 1f;
            target = Test_Target.Player;
        }
        else if (monster != null)
        {
            monster.agent.speed /= (1f - amount);
            target = Test_Target.Monster;
        }
        else
        {
            target = Test_Target.None;
        }
        Debug.Log($"{gameObject.name} is Rollback");
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
    public Test_StatusEffectType type;
    public ParticleSystem effect;
}
