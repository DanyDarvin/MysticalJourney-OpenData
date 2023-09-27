using System;
using Common.Npc.Enemy.Abstract.Factories;
using StaticData.Models;
using UniRx;

namespace Common.Npc.Enemy.Abstract.Components
{
    public interface IEnemyDeath : IDisposable
    {
        IObservable<Unit> OnRelease { get; }
        void Initialize(IEnemy enemy, IEnemyFactory enemyFactory, EnemyTypeId enemyTypeId);
    }
}