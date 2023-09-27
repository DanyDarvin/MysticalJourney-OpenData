using Common.Assets.Addressable.Providers;
using UnityEngine;
using Zenject;

namespace Common.Assets.Addressable.Installers
{
    [CreateAssetMenu(fileName = "AssetProviderInstaller", menuName = "Installers/AssetProviderInstaller")]
    public class AssetProviderInstaller : ScriptableObjectInstaller<AssetProviderInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
        }
    }
}