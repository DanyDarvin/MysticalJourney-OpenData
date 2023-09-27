using System.Collections.Generic;
using Common.Npc.Enemy.Abstract.Components;
using Common.Npc.Enemy.Abstract.Factories;
using Common.Pools.Abstract;
using StaticData.Models;
using UnityEngine;
using UnityEngine.Pool;

namespace Npc.Enemy.Factories
{
    public class EnemyPool : IPool<EnemyTypeId, IEnemy>
    {
        private const bool CollectionChecks = true;

        private readonly Dictionary<EnemyTypeId, IObjectPool<IEnemy>> _enemyPools;
        private readonly IEnemyFactory _enemyFactory;
        private readonly Transform _parent;
        private readonly int _defaultCapacity;
        private readonly int _maxPoolSize;

        public EnemyPool(IEnemyFactory enemyFactory, Transform parent, int maxPoolSize, int defaultCapacity)
        {
            _enemyFactory = enemyFactory;

            _enemyPools = new Dictionary<EnemyTypeId, IObjectPool<IEnemy>>();
            _maxPoolSize = maxPoolSize;
            _defaultCapacity = defaultCapacity;
            _parent = parent;
        }

        public void CreatePool(EnemyTypeId monsterTypeId)
        {
            if (_enemyPools.ContainsKey(monsterTypeId) is false)
            {
                ObjectPool<IEnemy> objectPool = new(
                    () => _enemyFactory.CreateEnemy(monsterTypeId, _parent),
                    OnTakeFromPool,
                    OnReturnToPool,
                    OnDestroyPoolObject, CollectionChecks, _defaultCapacity, _maxPoolSize);

                _enemyPools.Add(monsterTypeId, objectPool);
            }

            foreach (var monsterPoolsKey in _enemyPools.Keys)
            {
                Release(monsterPoolsKey, TryGetObject(monsterPoolsKey));
            }
        }

        public IEnemy TryGetObject(EnemyTypeId monsterTypeId) =>
            _enemyPools.TryGetValue(monsterTypeId, out var monsterPool) ? monsterPool.Get() : null;

        public void Release(EnemyTypeId typeId, IEnemy enemy) => _enemyPools[typeId].Release(enemy);

        public void Cleanup()
        {
            foreach (var key in _enemyPools.Keys) _enemyPools[key].Clear();
        }

        private static void OnTakeFromPool(IEnemy enemy) => enemy.Transform().gameObject.SetActive(true);

        private static void OnReturnToPool(IEnemy enemy) => enemy.Transform().gameObject.SetActive(false);

        private static void OnDestroyPoolObject(IEnemy enemy) => Object.Destroy(enemy.Transform().gameObject);
    }
}