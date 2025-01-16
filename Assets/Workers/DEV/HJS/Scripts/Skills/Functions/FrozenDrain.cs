using System.Collections.Generic;
using UnityEngine;

public class FrozenDrain : MonoBehaviour
{
    private Dictionary<GameObject, float> effectsDic = new Dictionary<GameObject, float>();
    [SerializeField] private AblityAdapter adapter;
    [SerializeField] private float slowPercent;

    private void Awake() => adapter = GetComponentInParent<AblityAdapter>();

    public void SetSpec()
    {
        var item = adapter.levelDic[SkillEnum.UniqueFunctionType.FrozenDrain];
        slowPercent = item.spec[item.Item1].y * 0.01f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!adapter.UniqueFuncDic.TryGetValue(SkillEnum.UniqueFunctionType.FrozenDrain, out bool value) || !value)
            return;

        if (other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        SetSpec();

        StatusEffect effect = other.GetComponentInChildren<StatusEffect>();
        if (effect != null)
        {
            if (!effectsDic.TryAdd(other.gameObject, slowPercent)) effectsDic[other.gameObject] = slowPercent;
            effect.Slow(slowPercent, out _);

            effect.StartEffect(StatusEffectType.Frost);
            SoundManager.PlaySFX(SoundManager.SoundData_S.BluechipSounds[5].AudioClip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!adapter.UniqueFuncDic.TryGetValue(SkillEnum.UniqueFunctionType.FrozenDrain, out bool value) || !value)
            return;

        if (other.gameObject.layer != LayerMask.NameToLayer("Monster"))
            return;

        StatusEffect effect = other.GetComponentInChildren<StatusEffect>();
        if (effect != null)
        {
            if (effectsDic.TryGetValue(other.gameObject, out float amount))
            {
                effect.Rollback(amount, out _);
                effect.StopEffect(StatusEffectType.Frost);
                effectsDic[other.gameObject] = 0;
            }
        }
    }
}
