using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] StatModel statModel;
    [SerializeField] ChangeInput input;
    [SerializeField] Loading loadingObject;
    public override void InstallBindings()
    {
        Container.Bind<StatModel>().FromInstance(statModel);
        Container.Bind<ChangeInput>().FromInstance(input);
        Container.Bind<Loading>().FromInstance(loadingObject);
        Container.Bind<SaveSlotData>().FromInstance(new SaveSlotData());
    }
    public void SaveSlotBind(SaveSlotData saveSlotData)
    {
        Container.Unbind<SaveSlotData>();
        Container.Bind<SaveSlotData>().FromInstance(saveSlotData);
    }
}