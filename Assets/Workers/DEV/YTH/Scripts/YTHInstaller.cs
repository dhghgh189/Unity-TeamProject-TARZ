using UnityEngine;
using Zenject;

public class YTHInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Container.Bind<MonsterSkillManager>().FromComponentInHierarchy().AsSingle();
    }
}