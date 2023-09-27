using System.Threading;
using Common.GameLogic.Abstract.Loaders;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace GameLogic.Loaders
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask Load(string sceneName, CancellationToken cancellationToken) =>
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).WithCancellation(cancellationToken);

        public async UniTask Unload(string sceneName, CancellationToken cancellationToken) =>
            await SceneManager.UnloadSceneAsync(sceneName).WithCancellation(cancellationToken);
    }
}