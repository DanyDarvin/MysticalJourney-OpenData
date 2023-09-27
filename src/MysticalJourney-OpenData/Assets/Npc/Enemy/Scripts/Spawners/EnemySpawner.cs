using System;
using System.Threading;
using Common.Npc.Enemy.Abstract;
using Common.Npc.Enemy.Abstract.Factories;
using Common.Npc.Enemy.Abstract.Spawners;
using Common.Player.Abstract.Factories;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Npc.Enemy.Spawners
{
    public class EnemySpawner : IEnemySpawner, IDisposable
    {
        public IObservable<Unit> OnEnemiesSlayed => _onEnemiesSlayed.AsObservable();

        private Subject<Unit> _onEnemiesSlayed;
        private BaseEnemySpawnPoint[] _enemyPoints;
        private IEnemyFactory _enemyFactory;
        private IPlayerFactory _playerFactory;
        private int _spawnEnemiesCount;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        [Inject]
        public void Construct(IPlayerFactory playerFactory, IEnemyFactory enemyFactory)
        {
            _playerFactory = playerFactory;
            _enemyFactory = enemyFactory;
        }

        public async UniTask Initialize(BaseEnemySpawnPoint[] enemyPoints, CancellationToken cancellationToken)
        {
            _onEnemiesSlayed = new Subject<Unit>();

            _enemyPoints = enemyPoints;
            _spawnEnemiesCount = enemyPoints.Length;

            await _enemyFactory.WarmUp(_enemyPoints, cancellationToken);
            _enemyFactory.SceneInitialize();

            InitializeEnemies();
        }

        private void InitializeEnemies()
        {
            foreach (var enemyPoint in _enemyPoints)
            {
                var enemy = _enemyFactory.GetEnemy(enemyPoint.EnemyTypeId(), enemyPoint.transform);
                enemy.Initialize(enemyPoint.EnemyTypeId(), _playerFactory.Player, enemyPoint.Waypoints());
                enemy.Death().OnRelease.Subscribe(OnReleased).AddTo(_cancellationTokenSource.Token);
            }
        }

        private void OnReleased(Unit unit)
        {
            _spawnEnemiesCount--;
            if (_spawnEnemiesCount <= 0)
            {
                _onEnemiesSlayed?.OnNext(Unit.Default);
            }
        }

        public void Dispose()
        {
            _onEnemiesSlayed?.Dispose();
            _onEnemiesSlayed = null;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}