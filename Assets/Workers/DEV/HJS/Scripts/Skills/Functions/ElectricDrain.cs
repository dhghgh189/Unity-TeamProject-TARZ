using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricDrain : MonoBehaviour, ISpec
{
    [SerializeField] private AblityAdapter adapter;
    [SerializeField] private WaitQueue damagedQueue;
    [SerializeField] private float damage;
    private IDamagable target;

    public void SetSpec(Spec spec, int level)
    {
        damage = spec.Power(level);
    }

    private void Awake() 
    {
        adapter = GetComponentInParent<AblityAdapter>();
        damagedQueue = GetComponent<WaitQueue>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!adapter.UniqueFuncDic.TryGetValue(SkillEnum.UniqueFunctionType.ElectricDrain, out bool value) || !value)
            return;

        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
        {
            if (damagedQueue.IsTargetInQueue(other.gameObject)) return;

            target = other.GetComponent<IDamagable>();
            if (target == null) return;

            // 중독 효과음
            target.TakeDamage(damage);

            damagedQueue.Add(other.gameObject, 1f);
        }
    }
}
