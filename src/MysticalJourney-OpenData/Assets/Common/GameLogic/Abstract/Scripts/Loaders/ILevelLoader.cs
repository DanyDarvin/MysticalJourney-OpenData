using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.GameLogic.Abstract.Loaders
{
    public interface ILevelLoader
    {
        string CurrentScene { get; }

        UniTask LoadLevel(string sceneName);
        UniTask ReloadCurrentLevel(CancellationToken cancellationToken);
    }
}