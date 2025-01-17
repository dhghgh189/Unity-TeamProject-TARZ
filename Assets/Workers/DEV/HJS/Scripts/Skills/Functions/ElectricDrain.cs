using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricDrain : MonoBehaviour
{
    [SerializeField] private DrainManager manager;
    [SerializeField] private AblityAdapter adapter;
    [SerializeField] private WaitQueue damagedQueue;
    [SerializeField] private SphereCollider coll;
    [SerializeField] private float damage;
    [SerializeField] private ParticleSystem effect;
    private IDamagable target;

    public void SetSpec()
    {
        var item = adapter.levelDic[SkillEnum.UniqueFunctionType.ElectricDrain];
        damage = item.Item2.InteractionDamage(item.Item1);
    }

    private void Awake() 
    {
        adapter = GetComponentInParent<AblityAdapter>();
        damagedQueue = GetComponent<WaitQueue>();
        manager = GetComponent<DrainManager>();
        coll = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        manager.OnStartEffectEvent.RemoveListener(StartEffect);
        manager.OnStartEffectEvent.AddListener(StartEffect);
        manager.OnStopEffectEvent.RemoveListener(StopEffect);
        manager.OnStopEffectEvent.AddListener(StopEffect);
    }

    private void Update()
    {
        if (!adapter.UniqueFuncDic.TryGetValue(SkillEnum.UniqueFunctionType.ElectricDrain, out bool value) || !value)
            return;

        //이펙트 드레인 크기만큼 펼치기
        effect.gameObject.transform.localScale = Vector3.one * coll.radius * 2f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!adapter.UniqueFuncDic.TryGetValue(SkillEnum.UniqueFunctionType.ElectricDrain, out bool value) || !value)
            return;

        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            SetSpec();

            if (damagedQueue.IsTargetInQueue(other.gameObject)) return;

            target = other.GetComponent<IDamagable>();
            if (target == null) return;

            // 전기 효과
            StatusEffect effect = other.GetComponentInChildren<StatusEffect>();
            if(effect != null) effect.StartEffect(StatusEffectType.Elec);

            // 데미지 효과음
            SoundManager.PlaySFX(SoundManager.SoundData_S.BluechipSounds[3].AudioClip);
            
            target.TakeDamage(damage);

            damagedQueue.Add(other.gameObject, 1f);
        }
    }

    private void StartEffect() { if (!effect.isPlaying) effect.Play(); }
    private void StopEffect() { if (effect.isPlaying) effect.Stop(); }
}
