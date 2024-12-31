using UnityEngine;
using Zenject;

public class MonsterFactoryInstaller : MonoInstaller
{
    [SerializeField] GameObject jake;
    [SerializeField] GameObject amber;
    [SerializeField] GameObject arnold;
    [SerializeField] GameObject bomber;
    [SerializeField] GameObject frogZombie;
    [SerializeField] GameObject jackTheRipper;
    [SerializeField] GameObject range;
    [SerializeField] GameObject reviveZombie;

    public override void InstallBindings()
    {
        Container.BindFactory<GameObject, MonsterFactory>().WithId("Jake").FromComponentInNewPrefab(jake).UnderTransformGroup("MonsterFactory/JakeFactory");
        Container.BindFactory<GameObject, MonsterFactory>().WithId("Amber").FromComponentInNewPrefab(amber).UnderTransformGroup("MonsterFactory/AmberFactory");
        Container.BindFactory<GameObject, MonsterFactory>().WithId("Arnold").FromComponentInNewPrefab(arnold).UnderTransformGroup("MonsterFactory/ArnoldFactory");
        Container.BindFactory<GameObject, MonsterFactory>().WithId("Bomber").FromComponentInNewPrefab(bomber).UnderTransformGroup("MonsterFactory/bomberFactory");
        Container.BindFactory<GameObject, MonsterFactory>().WithId("FrogZombie").FromComponentInNewPrefab(frogZombie).UnderTransformGroup("MonsterFactory/FrogZombieFactory");
        Container.BindFactory<GameObject, MonsterFactory>().WithId("JackTheRipper").FromComponentInNewPrefab(jackTheRipper).UnderTransformGroup("MonsterFactory/JackTheRipperFactory");
        Container.BindFactory<GameObject, MonsterFactory>().WithId("Range").FromComponentInNewPrefab(range).UnderTransformGroup("MonsterFactory/RangeFactory");
        Container.BindFactory<GameObject, MonsterFactory>().WithId("ReviveZombie").FromComponentInNewPrefab(reviveZombie).UnderTransformGroup("MonsterFactory/ReviveZombieFactory");
    }
}
public class MonsterFactory : PlaceholderFactory<GameObject>
{
    public override GameObject Create()
    {
        return base.Create();
    }

}