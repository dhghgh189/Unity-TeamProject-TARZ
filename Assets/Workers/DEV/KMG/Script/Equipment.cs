using UnityEngine;
using Zenject;

public class Equipment : MonoBehaviour
{
    [Inject] StatModel statModel;
    // 장착된 장비를 보관하는 배열
    [SerializeField] Gear[] equipmentGears = new Gear[(int)Part.Size];

    public void ChangeGear(Gear gear)
    {
        // 해당 파츠를 이미 장착한 상태라면 해당 장비의 능력치만큼 스탯을 감소
        if (equipmentGears[(int)gear.Part])
        {
            foreach (GearAbility gearAbility in equipmentGears[(int)gear.Part].Abilities)
            {
                statModel.SetAbility(gearAbility.ability, -gearAbility.value);
            }
        }
        // 장비의 능력치 만큼 스탯을 증가
        foreach (GearAbility gearAbility in gear.Abilities)
        {
            statModel.SetAbility(gearAbility.ability, gearAbility.value);
            equipmentGears[(int)gear.Part] = gear;
        }
    }
}
