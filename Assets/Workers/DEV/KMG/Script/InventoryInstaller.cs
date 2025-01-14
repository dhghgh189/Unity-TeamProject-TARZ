using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [Inject] SaveSlotData saveSlotData;
    public override void InstallBindings()
    {
        Container.Bind<InGameSaveData>().FromInstance(saveSlotData.InGameSaveData);
        Container.Bind<SaveManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Equipment>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Inventory>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ArmUpgradManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UI_InventorySlots>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<UI_EquipmentSlot>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<AblityAdapter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerSkillHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ChangeInput>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UI_Merchant>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ObjectPool_other>().FromInstance(this.gameObject.GetComponentInParent<ObjectPool_other>());
        Container.Bind<DamagePopUpManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerStateTransfer>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EffectManager>().FromComponentInHierarchy().AsSingle();

        GameObject DropPool = new GameObject("DropPool");
        Container.Bind<Transform>().FromInstance(DropPool.transform);
    }
}