using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Modules.AssetsManagement.AddressablesOperations
{
    public sealed class AddressablesService : IDisposable, IAddressablesService
    {
        private readonly Dictionary<string, AsyncOperationHandle> _assetRequests = new();

        public void Dispose()
        {
            foreach (AsyncOperationHandle handler in _assetRequests.Values)
                Addressables.Release(handler);
        }

        public async UniTask InitializeAsync() => 
            await Addressables.InitializeAsync().ToUniTask();

        public async UniTask<TAsset[]> LoadAsync<TAsset>(IEnumerable<AssetReference> assetReferences) 
            where TAsset : UnityEngine.Object
        {
            return await LoadByAddressAsync<TAsset>(assetReferences
                .Select(assetReference => assetReference.AssetGUID));
        }

        public async UniTask<TAsset> LoadAsync<TAsset>(AssetReference assetReference) where TAsset : UnityEngine.Object
        {
            return await LoadByAddressAsync<TAsset>(assetReference.AssetGUID);
        }

        public async UniTask<TAsset> LoadByAddressAsync<TAsset>(string assetAddress) where TAsset : UnityEngine.Object
        {
            AsyncOperationHandle handle = CreateAsyncOperationhandle<TAsset>(assetAddress);

            await handle.ToUniTask();
            TAsset loadedAsset = handle.Result as TAsset;

            if (loadedAsset == null)
            {
                ReleaseByAddress(assetAddress);
                throw new Exception($"Choosed asset type from address {assetAddress} is not exists");
            }
            
            return loadedAsset;
        }

        public async UniTask<TAsset[]> LoadByAddressAsync<TAsset>(IEnumerable<string> assetAddresses)
            where TAsset : UnityEngine.Object
        {
            List<UniTask<TAsset>> tasks = new List<UniTask<TAsset>>(assetAddresses.Count());

            foreach (string assetAddress in assetAddresses) 
                tasks.Add(LoadByAddressAsync<TAsset>(assetAddress));

            return await UniTask.WhenAll(tasks);
        }

        public async UniTask<TAsset[]> LoadByLabelAsync<TAsset>(string label) where TAsset : UnityEngine.Object
        {
            List<string> assetsAddresses = await GetAssetsAddressesByLabelAsync(label, typeof(TAsset));

            if (assetsAddresses == null)
                throw new Exception($"Asset addresses for choosed type with label '{label}' was not found");

            TAsset[] assets = await LoadByAddressAsync<TAsset>(assetsAddresses);

            return assets;
        }

        public void Release(IEnumerable<AssetReference> assetReferences)
        {
            foreach (AssetReference assetReference in assetReferences)
                Release(assetReference);
        }

        public void Release(AssetReference assetReference) =>
            ReleaseByAddress(assetReference.AssetGUID);

        public async UniTask ReleaseByLabel<TAsset>(string label) where TAsset : UnityEngine.Object
        {
            List<string> assetsAddresses = await GetAssetsAddressesByLabelAsync(label, typeof(TAsset));
            
            foreach (string assetAddress in assetsAddresses)
                ReleaseByAddress(assetAddress);
        }

        public void ReleaseByAddress(string address)
        {
            if (_assetRequests.TryGetValue(address, out var handler))
            {
                if (handler.IsValid())
                    Addressables.Release(handler);
                
                _assetRequests.Remove(address);
            }
        }

        private AsyncOperationHandle CreateAsyncOperationhandle<TAsset>(string address) 
            where TAsset : UnityEngine.Object
        {
            AsyncOperationHandle handle;

            if (_assetRequests.TryGetValue(address, out handle) == false)
            {
                handle = Addressables.LoadAssetAsync<TAsset>(address);
                _assetRequests.Add(address, handle);
            }

            return handle;
        }

        private async UniTask<List<string>> GetAssetsAddressesByLabelAsync(string label, Type type = null)
        {
            AsyncOperationHandle<IList<IResourceLocation>> operationHandle = Addressables
                .LoadResourceLocationsAsync(label, type);

            IList<IResourceLocation> resourcesLocations = await operationHandle.ToUniTask();

            List<string> assetsAddresses = new List<string>(resourcesLocations.Count);

            foreach (var resourceLocation in resourcesLocations) 
                assetsAddresses.Add(resourceLocation.PrimaryKey);
            
            Addressables.Release(operationHandle);

            return assetsAddresses;
        }
    }
}