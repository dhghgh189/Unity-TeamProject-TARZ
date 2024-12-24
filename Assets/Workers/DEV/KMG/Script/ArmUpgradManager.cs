using UnityEngine;
using Zenject;

public class ArmUpgradManager : MonoBehaviour
{
    [Inject] SaveData saveData;
    [Inject] StatModel statModel;
    [Inject] SaveManager saveManager;

    private void Start()
    {
        // 세이브 데이터의 업글 정보를 순회 하면서 모델에 반영
        foreach (ArmUpgrade item in saveData.ArmUpgradeDatas)
        {
            statModel.SetAbility(item.UpgradeAbility, item.UpgradeValue);
        }
    }
    // UI_ArmUpgrade의 버튼이 클릭되면 스탯을 반영하고 바로 저장함
    public bool ArmUpgredeExecute(int upNumber, AdditionAbility ability, float value, float cost)
    {
        if (statModel.Chip < cost) return false;

        statModel.SetAbility(ability, value);
        saveData.ArmUpgradeDatas.Add(new ArmUpgrade(upNumber, ability, value));
        saveManager.Save();

        return true;
    }
}
