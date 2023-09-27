using System.Threading;
using Common.Ui.Screens.Components;
using Cysharp.Threading.Tasks;

namespace Ui.Screens.GameOverScreen
{
    public interface IGameOverScreenComponent : IScreenComponent
    {
        UniTask WaitForRetryButtonClick(CancellationToken cancellationToken);
    }
}