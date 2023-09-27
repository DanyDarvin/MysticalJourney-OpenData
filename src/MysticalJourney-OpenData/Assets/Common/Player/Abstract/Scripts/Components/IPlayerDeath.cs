using System;
using UniRx;

namespace Common.Player.Abstract.Components
{
    public interface IPlayerDeath : IDisposable
    {
        public void Initialize();
        IObservable<Unit> OnRelease { get; }
    }
}