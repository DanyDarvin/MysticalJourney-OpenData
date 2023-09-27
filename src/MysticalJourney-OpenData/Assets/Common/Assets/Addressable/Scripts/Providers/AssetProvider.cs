using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Common.Assets.Addressable.Providers
{
    [UsedImplicitly]
    public class AssetProvider : IAssets
    {
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async UniTask<T> Load<T>(AssetReference assetReference, CancellationToken cancellationToken)
            where T : class
        {
            if (_handles.TryGetValue(assetReference.AssetGUID, out var completedHandle))
            {
                return completedHandle.First().Result as T;
            }

            return await RunWithCacheOnComplete(
                handle: Addressables.LoadAssetAsync<T>(assetReference),
                cacheKey: assetReference.AssetGUID,
                cancellationToken);
        }

        public async UniTask<T> Load<T>(string address, CancellationToken cancellationToken) where T : class
        {
            if (_handles.TryGetValue(address, out var completedHandle))
            {
                return completedHandle.First().Result as T;
            }

            return await RunWithCacheOnComplete(
                handle: Addressables.LoadAssetAsync<T>(address),
                cacheKey: address,
                cancellationToken);
        }

        public void Cleanup()
        {
            foreach (var handle in _handles.Values.SelectMany(resourceHandles => resourceHandles))
            {
                Addressables.Release(handle);
            }

            _handles.Clear();
        }

        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey,
            CancellationToken cancellationToken) where T : class
        {
            var handleTask = await handle.WithCancellation(cancellationToken);
            AddHandle(cacheKey, handle);
            return handleTask;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out var resourceHandle))
            {
                resourceHandle = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandle;
            }

            resourceHandle.Add(handle);
        }
    }
}