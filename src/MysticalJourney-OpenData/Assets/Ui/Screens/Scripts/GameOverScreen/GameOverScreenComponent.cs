using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Screens.GameOverScreen
{
    public class GameOverScreenComponent : MonoBehaviour, IGameOverScreenComponent
    {
        [SerializeField] private Button _retryButton;

        public UniTask WaitForRetryButtonClick(CancellationToken cancellationToken) =>
            _retryButton.OnClickAsync(cancellationToken);

        public UniTask Initialize(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public UniTask Run(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            DestroyImmediate(gameObject);
            return UniTask.CompletedTask;
        }
    }
}