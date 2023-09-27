using System.Threading;
using Common.GameLogic.Abstract;
using Common.Ui.Screens;
using Common.Ui.Screens.Mediators;
using Cysharp.Threading.Tasks;
using StaticData.Models;
using Zenject;

namespace Ui.Screens.LevelWonScreen
{
    public class LevelWonScreenMediator : BaseScreenMediator<ILevelWonScreenComponent>
    {
        private readonly IScreen _screen;
        private readonly IGameLoop _gameLoop;

        [Inject]
        public LevelWonScreenMediator(
            IScreen screen,
            IGameLoop gameLoop
        )
        {
            _screen = screen;
            _gameLoop = gameLoop;
        }

        protected override UniTask InitializeMediator(CancellationToken cancellationToken)
        {
            return ScreenComponent.Initialize(cancellationToken);
        }

        protected override async UniTask RunMediator(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                await ScreenComponent.Run(cancellationToken);
                await ScreenComponent.WaitForRetryButtonClick(cancellationToken);
                await _screen.Close(ScreenType.LevelWon, cancellationToken);
                _gameLoop.RetryCurrentLevel().Forget();
            }
        }

        protected override UniTask DisposeMediator()
        {
            return ScreenComponent.DisposeAsync();
        }
    }
}