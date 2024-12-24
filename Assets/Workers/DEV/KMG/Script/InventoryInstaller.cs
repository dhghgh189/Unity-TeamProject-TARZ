using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SaveManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Equipment>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Inventory>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ArmUpgradManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UI_InventorySlots>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<UI_EquipmentSlot>().FromComponentsInHierarchy().AsSingle();
    }
}