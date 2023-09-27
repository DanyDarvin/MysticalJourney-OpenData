using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Ui.Screens.Components
{
    public interface IScreenComponent : IUniTaskAsyncDisposable
    {
        UniTask Initialize(CancellationToken cancellationToken);
        UniTask Run(CancellationToken cancellationToken);
    }
}