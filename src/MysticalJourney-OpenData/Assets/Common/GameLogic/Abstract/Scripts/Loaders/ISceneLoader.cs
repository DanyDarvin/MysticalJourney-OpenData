using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.GameLogic.Abstract.Loaders
{
    public interface ISceneLoader
    {
        UniTask Load(string sceneName, CancellationToken cancellationToken);
        UniTask Unload(string sceneName, CancellationToken cancellationToken);
    }
}