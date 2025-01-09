using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "BagSkillDataContainerInstaller", menuName = "Installers/BagSkillDataContainerInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    [SerializeField] BagSkillContainerSO container;
    public override void InstallBindings()
    {
        Container.Bind<BagSkillContainerSO>().FromInstance(container);
    }
}