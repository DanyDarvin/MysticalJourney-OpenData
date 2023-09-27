using System.Threading;
using Common.Assets.Addressable;
using Common.Ui.Screens.Components;
using Common.Ui.Screens.Mediators;
using Cysharp.Threading.Tasks;
using StaticData;
using StaticData.Models;
using UnityEngine;
using Zenject;

namespace Common.Ui.Screens.Factories
{
    public interface IUiFactory
    {
        UniTask CreateScreensRoot(CancellationToken cancellationToken);

        UniTask<IScreenMediator> InitializeScreen<TScreenMediator>(ScreenType screenType,
            CancellationToken cancellationToken)
            where TScreenMediator : IScreenMediator;
    }

    public class UiFactory : IUiFactory
    {
        private readonly IAssets _assets;
        private readonly DiContainer _container;
        private readonly IStaticData _staticData;
        private Transform _screensRoot;

        [Inject]
        public UiFactory(
            IAssets assets,
            DiContainer container,
            IStaticData staticData
        )
        {
            _assets = assets;
            _container = container;
            _staticData = staticData;
        }

        public async UniTask CreateScreensRoot(CancellationToken cancellationToken) =>
            _screensRoot = _container
                .InstantiatePrefabForComponent<Transform>(await LoadScreen(ScreenType.ScreensRoot,
                    cancellationToken));

        public async UniTask<IScreenMediator> InitializeScreen<TScreenMediator>(ScreenType screenType,
            CancellationToken cancellationToken)
            where TScreenMediator : IScreenMediator
        {
            var screenComponent = _container.InstantiatePrefabForComponent<IScreenComponent>(
                await LoadScreen(screenType, cancellationToken),
                _screensRoot);

            var screenMediator = _container.Resolve<TScreenMediator>();
            await screenMediator.Initialize(screenComponent, cancellationToken);
            return screenMediator;
        }

        private async UniTask<GameObject> LoadScreen(ScreenType screenType, CancellationToken cancellationToken) =>
            await _assets.Load<GameObject>(_staticData.ForScreen(screenType).PrefabReference, cancellationToken);
    }
}