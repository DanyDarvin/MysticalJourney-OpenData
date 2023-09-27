using System;
using Common.Npc.Enemy.Abstract.Components;
using Common.Npc.Enemy.Abstract.Factories;
using Common.Stats.Abstract;
using DG.Tweening;
using StaticData.Models;
using UniRx;

namespace Npc.Enemy.Components
{
    public class EnemyDeath : IEnemyDeath
    {
        public IObservable<Unit> OnRelease => _onReleased.AsObservable();
        private Subject<Unit> _onReleased = new();

        private IEnemyFactory _enemyFactory;
        private EnemyTypeId _enemyTypeId;
        private IHealth _health;
        private CompositeDisposable _compositeDisposable;

        //todo move to weapon object
        private float releaseDelay = 3f;
        private IEnemy _enemy;

        public void Initialize(IEnemy enemy, IEnemyFactory enemyFactory, EnemyTypeId enemyTypeId)
        {
            _enemy = enemy;
            _health = enemy.Health();
            _enemyTypeId = enemyTypeId;
            _enemyFactory = enemyFactory;

            _compositeDisposable = new CompositeDisposable();

            _health.Current.Subscribe(OnHealthChanged).AddTo(_compositeDisposable);
        }

        private void OnHealthChanged(float current)
        {
            if (current <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            DOVirtual
                .Float(0f, releaseDelay, releaseDelay, _ => { })
                .OnComplete(() => { _enemyFactory.ReleaseEnemy(_enemyTypeId, _enemy); })
                .SetAutoKill(true);

            _onReleased?.OnNext(Unit.Default);

            Dispose();
        }

        public void Dispose()
        {
            _onReleased?.Dispose();
            _onReleased = null;

            _compositeDisposable?.Clear();
            _compositeDisposable?.Dispose();
        }
    }
}