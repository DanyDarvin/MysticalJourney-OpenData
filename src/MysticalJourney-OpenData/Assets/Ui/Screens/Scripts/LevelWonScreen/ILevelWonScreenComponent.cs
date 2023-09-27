using System.Threading;
using Common.Ui.Screens.Components;
using Cysharp.Threading.Tasks;

namespace Ui.Screens.LevelWonScreen
{
    public interface ILevelWonScreenComponent : IScreenComponent
    {
        UniTask WaitForRetryButtonClick(CancellationToken cancellationToken);
    }
}