using System;
using System.Collections.Generic;
using System.Threading;
using Common.Assets.Addressable;
using Common.Npc.Enemy.Abstract;
using Common.Npc.Enemy.Abstract.Components;
using Common.Npc.Enemy.Abstract.Factories;
using Cysharp.Threading.Tasks;
using StaticData;
using StaticData.Models;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Npc.Enemy.Factories
{
    public class EnemyFactory : IEnemyFactory
    {
        //todo get from level data
        private const string EnemyPoolParent = "EnemyPool";
        private const int MaxPoolSize = 10;
        private const int PoolCapacity = 10;

        private readonly Dictionary<EnemyTypeId, GameObject> _loadedEnemiesPrefabs = new();
        private readonly DiContainer _container;
        private readonly IAssets _assets;
        private readonly IStaticData _staticData;

        private EnemyPool _enemyPool;

        public EnemyFactory(
            DiContainer container,
            IAssets assets,
            IStaticData staticData
        )
        {
            _container = container;
            _assets = assets;
            _staticData = staticData;
        }

        public async UniTask WarmUp(BaseEnemySpawnPoint[] enemyPoints, CancellationToken cancellationToken)
        {
            foreach (var spawnerDetails in enemyPoints)
            {
                if (_loadedEnemiesPrefabs.ContainsKey(spawnerDetails.EnemyTypeId()) is false)
                {
                    var loadMonster = await LoadEnemy(spawnerDetails.EnemyTypeId(), cancellationToken);

                    _loadedEnemiesPrefabs.Add(spawnerDetails.EnemyTypeId(), loadMonster);
                }
            }
        }

        public void SceneInitialize()
        {
            _enemyPool = new EnemyPool(this, new GameObject(EnemyPoolParent).transform, MaxPoolSize, PoolCapacity);

            foreach (var key in _loadedEnemiesPrefabs.Keys)
            {
                _enemyPool.CreatePool(key);
            }
        }

        public IEnemy CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
            if (_loadedEnemiesPrefabs.TryGetValue(typeId, out var gameObject))
            {
                return _container.InstantiatePrefabForComponent<IEnemy>(gameObject, parent);
            }

            Debug.LogException(new Exception("Can't find in loadedEnemies"));
            return default;
        }

        public IEnemy GetEnemy(EnemyTypeId typeId, Transform parent)
        {
            var enemyData = _staticData.ForEnemy(typeId);
            var enemy = TryGetPooledEnemy(typeId);

            SetupEnemyData(enemy, enemyData);

            enemy.Health().Initialize(enemyData.Hp, enemyData.Hp);
            enemy.Death().Initialize(enemy, this, typeId);

            enemy.Transform().position = parent.position;
            enemy.Transform().rotation = Quaternion.identity;

            WrapToNavMesh(parent, enemy);
            return enemy;
        }

        public void ReleaseEnemy(EnemyTypeId typeId, IEnemy enemy) => _enemyPool.Release(typeId, enemy);
        public void Cleanup() => _enemyPool?.Cleanup();

        private static void SetupEnemyData(IEnemy enemy, EnemyStaticData enemyData)
        {
            enemy.MinimalChaseDistance = enemyData.MinimalChaseDistance;
            enemy.Damage = enemyData.Damage;
            enemy.PatrolMoveSpeed = enemyData.PatrolMoveSpeed;
            enemy.ChaseMoveSpeed = enemyData.ChaseMoveSpeed;
            enemy.AttackCooldown = enemyData.AttackCooldown;
            enemy.IdleCooldown = enemyData.IdleCooldown;
            enemy.PatrolRemainingDistance = enemyData.PatrolRemainingDistance;
        }

        private static void WrapToNavMesh(Transform parent, IEnemy enemy)
        {
            if (NavMesh.SamplePosition(parent.position, out var myNavHit, 100, -1))
                enemy.Agent().Warp(myNavHit.position);
        }

        private IEnemy TryGetPooledEnemy(EnemyTypeId typeId) =>
            _enemyPool.TryGetObject(typeId);

        private async UniTask<GameObject> LoadEnemy(EnemyTypeId monsterTypeId, CancellationToken cancellationToken) =>
            await _assets
                .Load<GameObject>(_staticData.ForEnemy(monsterTypeId).PrefabReference, cancellationToken);
    }
}