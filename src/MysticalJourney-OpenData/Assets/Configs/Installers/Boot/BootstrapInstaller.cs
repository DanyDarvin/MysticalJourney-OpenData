using Common.GameLogic.Abstract;
using StaticData;
using UnityEngine;
using Zenject;

namespace Configs.Installers.Boot
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private Texture2D _crosshair;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
        }

        public async void Initialize()
        {
            InitCursor();

            await Container.Resolve<IGameLoop>()
                .StartGameLevel(Constants.LevelSceneName);
        }

        private void InitCursor()
        {
            if (_crosshair is null) return;
            Cursor.SetCursor(_crosshair, Vector2.zero, CursorMode.Auto);
        }
    }
}