using System.Threading;
using Common.Ui.Screens.Components;
using Cysharp.Threading.Tasks;

namespace Common.Ui.Screens.Mediators
{
    public interface IScreenMediator : IUniTaskAsyncDisposable
    {
        UniTask Initialize(IScreenComponent screenComponent, CancellationToken cancellationToken);
        UniTask Run(CancellationToken cancellationToken);
    }
}