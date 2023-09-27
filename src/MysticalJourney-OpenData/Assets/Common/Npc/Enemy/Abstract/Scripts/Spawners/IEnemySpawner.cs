using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Common.Npc.Enemy.Abstract.Spawners
{
    public interface IEnemySpawner
    {
        IObservable<Unit> OnEnemiesSlayed { get; }
        UniTask Initialize(BaseEnemySpawnPoint[] enemyPoints, CancellationToken cancellationToken);
    }
}