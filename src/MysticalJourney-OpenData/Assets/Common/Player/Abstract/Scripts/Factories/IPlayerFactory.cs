using System.Threading;
using Common.Player.Abstract.Components;
using Common.Player.Abstract.Hud;
using Cysharp.Threading.Tasks;

namespace Common.Player.Abstract.Factories
{
    public interface IPlayerFactory
    {
        IPlayer Player { get; }
        UniTask<IHud> InitializeHud(IPlayer player, CancellationToken cancellationToken);
        UniTask<IPlayer> InitializePlayer(string sceneName, CancellationToken cancellationToken);
        void DestroyPlayer();
        void DestroyHud();
    }
}