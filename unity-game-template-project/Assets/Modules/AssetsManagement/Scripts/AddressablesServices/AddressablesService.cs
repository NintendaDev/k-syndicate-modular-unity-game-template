using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Modules.AssetsManagement.AddressablesServices
{
    public sealed class AddressablesService : IAddressablesService
    {
        private readonly Dictionary<string, AsyncOperationHandle> _assetRequests = new();

        public async UniTask InitializeAsync() => 
            await Addressables.InitializeAsync().ToUniTask();

        public async UniTask<TAsset> LoadAsync<TAsset>(AssetReferenceT<TAsset> assetReference)
            where TAsset : UnityEngine.Object
        {
            return await LoadByAddressAsync<TAsset>(assetReference.AssetGUID);
        }

        public async UniTask<TAsset> LoadByAddressAsync<TAsset>(string address) where TAsset : UnityEngine.Object
        {
            AsyncOperationHandle handle = CreateAsyncOperationhandle<TAsset>(address);

            await handle.ToUniTask();
            TAsset loadedAsset = handle.Result as TAsset;

            if (loadedAsset == null)
                throw new Exception($"Choosed asset type from address {address} is not exists");

            return loadedAsset;
        }

        public async UniTask<TAsset> LoadAsync<TAsset>(AssetReference assetReference)
            where TAsset : UnityEngine.Object
        {
            return await LoadByAddressAsync<TAsset>(assetReference.AssetGUID);
        }
            

        public async UniTask<TAsset[]> LoadByAddressAsync<TAsset>(IEnumerable<string> addresses)
            where TAsset : UnityEngine.Object
        {
            List<UniTask<TAsset>> tasks = new List<UniTask<TAsset>>(addresses.Count());

            foreach (var key in addresses) 
                tasks.Add(LoadByAddressAsync<TAsset>(key));

            return await UniTask.WhenAll(tasks);
        }

        public async UniTask<TAsset> LoadOneByLabelAsync<TAsset>(string label) where TAsset : UnityEngine.Object
        {
            TAsset[] assets = await LoadByLabelAsync<TAsset>(label);

            int maxAssetsCount = 1;

            if (assets.Length > maxAssetsCount)
                throw new Exception($"More than one assets with choosed type was found");

            if (assets.Length == 0)
                throw new Exception($"No assets with choosed type was found");

            return assets.First();
        }

        public async UniTask<TAsset[]> LoadByLabelAsync<TAsset>(string label) where TAsset : UnityEngine.Object
        {
            List<string> assetsAddresses = await GetAssetsAddressesByLabelAsync<TAsset>(label);

            if (assetsAddresses == null)
                throw new Exception($"Asset addresses for choosed type with label '{label}' was not found");

            TAsset[] assets = await LoadByAddressAsync<TAsset>(assetsAddresses);

            return assets;
        }

        public async UniTask WarmupAssetsByLabelAsync(string label)
        {
            var assetsList = await GetAssetsAddressesByLabelAsync(label);
            await LoadByAddressAsync<UnityEngine.Object>(assetsList);
        }

        public async UniTask<List<string>> GetAssetsAddressesByLabelAsync<TAsset>(string label) => 
            await GetAssetsAddressesByLabelAsync(label, typeof(TAsset));

        public async UniTask<List<string>> GetAssetsAddressesByLabelAsync(string label, Type type = null)
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

        public async UniTask ReleaseAssetsByLabelAsync(string label)
        {
            var assetsList = await GetAssetsAddressesByLabelAsync(label);

            foreach (var assetAddress in assetsList)
                Release(assetAddress);
        }

        public void Release(AssetReference assetReference) =>
            Release(assetReference.AssetGUID);

        public void Release(string address)
        {
            if (_assetRequests.TryGetValue(address, out var handler))
            {
                Addressables.Release(handler);
                _assetRequests.Remove(address);
            }
        }

        public void Release<TAsset>(AssetReferenceT<TAsset> assetReference) where TAsset : UnityEngine.Object =>
            Release(assetReference.AssetGUID);

        public void Cleanup()
        {
            foreach (var assetRequest in _assetRequests) 
                Addressables.Release(assetRequest.Value);
            
            _assetRequests.Clear();
        }

        private AsyncOperationHandle CreateAsyncOperationhandle<TAsset>(string address) where TAsset : UnityEngine.Object
        {
            AsyncOperationHandle handle;

            if (_assetRequests.TryGetValue(address, out handle) == false)
            {
                handle = Addressables.LoadAssetAsync<TAsset>(address);
                _assetRequests.Add(address, handle);
            }

            return handle;
        }
    }
}