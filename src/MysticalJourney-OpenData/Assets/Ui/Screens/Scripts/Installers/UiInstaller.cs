using Common.Ui.Screens.Factories;
using Ui.Screens.GameOverScreen;
using Ui.Screens.LevelWonScreen;
using UnityEngine;
using Zenject;
using Screen = Common.Ui.Screens.Screen;

namespace Ui.Screens.Installers
{
    [CreateAssetMenu(fileName = "UiInstaller", menuName = "Installers/UiInstaller")]
    public class UiInstaller : ScriptableObjectInstaller<UiInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UiFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Screen>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameOverScreenMediator>().AsTransient();
            Container.BindInterfacesAndSelfTo<LevelWonScreenMediator>().AsTransient();
        }
    }
}