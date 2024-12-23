using UnityEngine;
using Zenject;

public class HJS_Installer : MonoInstaller
{
    SaveData saveData = new();
    public override void InstallBindings()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveData")))
        {
            saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveData"));
            Debug.Log(JsonUtility.ToJson(saveData, true));
        }
        Container.Bind<SaveData>().FromInstance(saveData);

        Container.Bind<StatModel>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Equipment>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Inventory>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AblityAdapter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UI_InventorySlots>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<UI_EquipmentSlot>().FromComponentsInHierarchy().AsSingle();
    }
}