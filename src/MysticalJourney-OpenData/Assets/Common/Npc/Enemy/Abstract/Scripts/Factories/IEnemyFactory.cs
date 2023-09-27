using System.Threading;
using Common.Npc.Enemy.Abstract.Components;
using Cysharp.Threading.Tasks;
using StaticData.Models;
using UnityEngine;

namespace Common.Npc.Enemy.Abstract.Factories
{
    public interface IEnemyFactory
    {
        UniTask WarmUp(BaseEnemySpawnPoint[] enemyPoints, CancellationToken cancellationToken);
        IEnemy GetEnemy(EnemyTypeId typeId, Transform parent);
        void ReleaseEnemy(EnemyTypeId typeId, IEnemy enemy);
        IEnemy CreateEnemy(EnemyTypeId typeId, Transform parent);
        void Cleanup();
        void SceneInitialize();
    }
}