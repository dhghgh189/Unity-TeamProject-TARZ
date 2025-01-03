using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] StatModel statModel;
    [SerializeField] ChangeInput input;
    [SerializeField] Loading loadingObject;
    private SaveData saveData = new();
    public override void InstallBindings()
    {
        //if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveData")))
        //{
        //    saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
        //    Debug.Log(JsonUtility.ToJson(saveData, true));
        //}
        Container.Bind<SaveData>().FromInstance(saveData);
        Container.Bind<StatModel>().FromInstance(statModel);
        Container.Bind<ChangeInput>().FromInstance(input);
        Container.Bind<Loading>().FromInstance(loadingObject);
    }
}