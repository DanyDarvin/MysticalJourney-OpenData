using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Common.Assets.Addressable
{
    public interface IAssets
    {
        void Initialize();
        UniTask<T> Load<T>(AssetReference assetReference, CancellationToken cancellationToken) where T : class;
        UniTask<T> Load<T>(string address, CancellationToken cancellationToken) where T : class;
        void Cleanup();
    }
}