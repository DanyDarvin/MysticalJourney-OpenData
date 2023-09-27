using System;
using System.Threading;
using Common.Player.Abstract.Components;
using Common.Stats.Abstract;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using StaticData.Models;
using UniRx;
using Zenject;

namespace Player.Components
{
    public class PlayerDeath : IPlayerDeath
    {
        public IObservable<Unit> OnRelease => _onReleased.AsObservable();

        private Subject<Unit> _onReleased;

        private IHealth _health;

        private CancellationTokenSource _cancellationTokenSource;

        //todo move to data object
        private float _releaseDelay = 3f;

        [Inject]
        public PlayerDeath(
            [Inject(Id = GameEntityType.Player)] IHealth health
        )
        {
            _health = health;
        }

        public void Initialize()
        {
            _onReleased = new Subject<Unit>();
            _cancellationTokenSource = new CancellationTokenSource();

            _health.Current.Subscribe(OnHealthChanged).AddTo(_cancellationTokenSource.Token);
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
                .Float(0f, _releaseDelay, _releaseDelay, _ => { })
                .OnComplete(() => { })
                .SetAutoKill(true);

            _onReleased?.OnNext(Unit.Default);

            Dispose();
        }

        public void Dispose()
        {
            _onReleased?.Dispose();
            _onReleased = null;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}