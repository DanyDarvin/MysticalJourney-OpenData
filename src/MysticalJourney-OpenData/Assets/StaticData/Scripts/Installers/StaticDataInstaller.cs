using UnityEngine;
using Zenject;

namespace StaticData.Installers
{
    [CreateAssetMenu(fileName = "StaticDataInstaller", menuName = "Installers/StaticDataInstaller")]
    public class StaticDataInstaller : ScriptableObjectInstaller<StaticDataInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StaticData>().AsSingle();
        }
    }
}