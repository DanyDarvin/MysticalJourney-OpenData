using GameLogic.Loaders;
using UnityEngine;
using Zenject;

namespace GameLogic.Installers
{
    [CreateAssetMenu(fileName = "LevelInstaller", menuName = "Installers/LevelInstaller")]
    public class LevelInstaller : ScriptableObjectInstaller<LevelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelLoader>().AsSingle().NonLazy();
        }
    }
}