using System.Threading;
using Common.Ui.Screens.Mediators;
using Cysharp.Threading.Tasks;
using StaticData.Models;

namespace Common.Ui.Screens
{
    public interface IScreen
    {
        UniTask Show<TScreenMediator>(ScreenType screenType, CancellationToken cancellationToken)
            where TScreenMediator : IScreenMediator;

        UniTask Close(ScreenType screenType, CancellationToken cancellationToken);
    }
}