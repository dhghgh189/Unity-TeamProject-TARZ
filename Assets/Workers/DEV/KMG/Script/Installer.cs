using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<StatModel>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Equipment>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UI_GearSlot>().FromComponentsInHierarchy().AsSingle();
    }
}