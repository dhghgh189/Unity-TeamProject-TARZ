using UnityEngine;
using Zenject;

public class YTHInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Container.Bind<CoroutineManager>().FromComponentInHierarchy().AsSingle();
    }
}