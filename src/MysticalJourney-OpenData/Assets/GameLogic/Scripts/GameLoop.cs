using System.Threading;
using Common.GameLogic.Abstract;
using Common.GameLogic.Abstract.Loaders;
using Common.Input.Abstract;
using Common.Npc.Enemy.Abstract.Spawners;
using Common.Player.Abstract.Components;
using Common.Ui.Screens;
using Cysharp.Threading.Tasks;
using StaticData.Models;
using Ui.Screens.GameOverScreen;
using Ui.Screens.LevelWonScreen;
using UniRx;
using Zenject;

namespace GameLogic
{
    public class GameLoop : IGameLoop
    {
        private readonly ILevelLoader _levelLoader;
        private readonly IPlayerDeath _playerDeath;
        private readonly IMouseLook _mouseLook;
        private readonly IMove _move;
        private readonly IPlayerAttack _playerAttack;
        private readonly IScreen _screen;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private IEnemySpawner _enemySpawner;

        private CancellationToken CancellationToken => _cancellationTokenSource.Token;

        [Inject]
        public GameLoop(
            ILevelLoader levelLoader,
            IPlayerDeath playerDeath,
            IMouseLook mouseLook,
            IMove move,
            IPlayerAttack playerAttack,
            IScreen screen,
            IEnemySpawner enemySpawner
        )
        {
            _enemySpawner = enemySpawner;
            _playerAttack = playerAttack;
            _screen = screen;
            _levelLoader = levelLoader;
            _playerDeath = playerDeath;
            _mouseLook = mouseLook;
            _move = move;
        }

        public async UniTask StartGameLevel(string sceneName)
        {
            await _levelLoader.LoadLevel(sceneName);

            InitializeLevelLogic();
        }

        public async UniTask RetryCurrentLevel()
        {
            DisablePlayerInput();
            await _levelLoader.ReloadCurrentLevel(CancellationToken);

            InitializeLevelLogic();
        }

        private void InitializeLevelLogic()
        {
            _playerDeath.OnRelease.Subscribe(OnPlayerDied).AddTo(CancellationToken);
            _enemySpawner.OnEnemiesSlayed.Subscribe(OnEnemiesSlayed).AddTo(CancellationToken);
            EnablePlayerInput();
        }

        private async void OnPlayerDied(Unit unit)
        {
            DisablePlayerInput();
            await _screen.Show<GameOverScreenMediator>(ScreenType.GameOver, CancellationToken);
        }

        private async void OnEnemiesSlayed(Unit unit)
        {
            DisablePlayerInput();
            await _screen.Show<LevelWonScreenMediator>(ScreenType.LevelWon, CancellationToken);
        }

        private void EnablePlayerInput()
        {
            _mouseLook.Enable();
            _move.Enable();
            _playerAttack.Enable();
        }

        private void DisablePlayerInput()
        {
            _mouseLook.Disable();
            _move.Disable();
            _playerAttack.Disable();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}