using System.Threading;
using Common.Ui.Screens.Components;
using Cysharp.Threading.Tasks;

namespace Common.Ui.Screens.Mediators
{
    public abstract class BaseScreenMediator<TScreenComponent> : IScreenMediator
        where TScreenComponent : IScreenComponent
    {
        protected TScreenComponent ScreenComponent;
        private CancellationToken CancellationToken => _cancellationTokenSource.Token;

        protected abstract UniTask InitializeMediator(CancellationToken cancellationToken);
        protected abstract UniTask RunMediator(CancellationToken cancellationToken);
        protected abstract UniTask DisposeMediator();

        private CancellationTokenSource _cancellationTokenSource = new();

        public UniTask Initialize(IScreenComponent screenComponent, CancellationToken cancellationToken)
        {
            ScreenComponent = (TScreenComponent)screenComponent;
            return InitializeMediator(CreateLinkedToken(cancellationToken));
        }

        public UniTask Run(CancellationToken cancellationToken)
        {
            return RunMediator(CreateLinkedToken(cancellationToken));
        }

        private CancellationToken CreateLinkedToken(CancellationToken cancellationToken)
        {
            var linkedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(CancellationToken, cancellationToken);

            return linkedTokenSource.Token;
        }

        public UniTask DisposeAsync()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            return DisposeMediator();
        }
    }
}