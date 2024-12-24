using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] StatModel statModel;
    private SaveData saveData = new();
    public override void InstallBindings()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveData")))
        {
            saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
            Debug.Log(JsonUtility.ToJson(saveData, true));
        }
        Container.Bind<SaveData>().FromInstance(saveData);
        Container.Bind<StatModel>().FromInstance(statModel);
        Container.Bind<AblityAdapter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerAttack>().FromComponentInHierarchy().AsSingle();
    }
}