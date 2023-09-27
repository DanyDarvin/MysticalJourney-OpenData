using System;
using System.Threading;
using Common.GameLogic.Abstract.Loaders;
using Common.Npc.Enemy.Abstract;
using Common.Npc.Enemy.Abstract.Spawners;
using Common.Player.Abstract.Components;
using Common.Player.Abstract.Factories;
using Common.Player.Abstract.Hud;
using Common.Ui.Screens.Factories;
using Cysharp.Threading.Tasks;
using StaticData;
using StaticData.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLogic.Loaders
{
    public class LevelLoader : ILevelLoader, IDisposable
    {
        public string CurrentScene => _currentScene;

        private readonly ISceneLoader _sceneLoader;
        private readonly IPlayerFactory _playerFactory;
        private readonly IEnemySpawner _enemySpawner;
        private readonly IUiFactory _uiFactory;
        private readonly IStaticData _staticData;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private CancellationToken CancellationToken => _cancellationTokenSource.Token;
        private string _currentScene;

        public LevelLoader(
            ISceneLoader sceneLoader,
            IPlayerFactory playerFactory,
            IEnemySpawner enemySpawner,
            IUiFactory uiFactory,
            IStaticData staticData
        )
        {
            _sceneLoader = sceneLoader;
            _playerFactory = playerFactory;
            _enemySpawner = enemySpawner;
            _uiFactory = uiFactory;
            _staticData = staticData;
        }

        public async UniTask LoadLevel(string sceneName)
        {
            _currentScene = sceneName;
            await _sceneLoader.Load(sceneName, CancellationToken);
            await InitGameWorld(CancellationToken);
        }

        private async UniTask InitGameWorld(CancellationToken cancellationToken)
        {
            var levelData = LevelStaticData();
            await InitializeScreensRoot(cancellationToken);
            var player = await InitializePlayer(levelData, cancellationToken);
            await InitializeHud(player, cancellationToken);
            await InitializeEnemy();
        }

        public async UniTask ReloadCurrentLevel(CancellationToken cancellationToken)
        {
            await _sceneLoader.Load(_currentScene, CancellationToken);
            _playerFactory.DestroyPlayer();
            _playerFactory.DestroyHud();
            var levelData = LevelStaticData();
            var player = await InitializePlayer(levelData, cancellationToken);
            await InitializeHud(player, cancellationToken);
            await InitializeEnemy();
        }

        private async UniTask InitializeEnemy()
        {
            var enemySpawnPoints = Object.FindObjectsByType<BaseEnemySpawnPoint>(FindObjectsSortMode.None);
            await _enemySpawner.Initialize(enemySpawnPoints, CancellationToken);
        }

        private async UniTask InitializeScreensRoot(CancellationToken cancellationToken) =>
            await _uiFactory.CreateScreensRoot(cancellationToken);

        private async UniTask<IHud> InitializeHud(IPlayer player, CancellationToken cancellationToken) =>
            await _playerFactory.InitializeHud(player, cancellationToken);

        private async UniTask<IPlayer>
            InitializePlayer(LevelStaticData levelData, CancellationToken cancellationToken) =>
            await _playerFactory.InitializePlayer(levelData.LevelKey, cancellationToken);

        private LevelStaticData LevelStaticData() =>
            _staticData.ForLevel(_currentScene);

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}